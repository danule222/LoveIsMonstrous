using Godot;
using GodotInk;

public partial class DialogueController : Control
{
	private GameController GCon;

	private RichTextLabel TXT_Name;
	private RichTextLabel TXT_Dial;
	private VBoxContainer VBX_Opts;
	private Panel PNL_Opts;
	private TextureRect IMG_Character;
	private TextureRect IMG_Background;
	private Character ActualCharacter;
	private bool FirstCharacter;

	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		TXT_Dial = GetNode<RichTextLabel>("Text/TXT_Dialogue");
		TXT_Name = GetNode<RichTextLabel>("Name/TXT_Name");
		VBX_Opts = GetNode<VBoxContainer>("Options/VBoxContainer");
		PNL_Opts = GetNode<Panel>("Options");
		IMG_Character = GetNode<TextureRect>("IMG_Character");
		IMG_Background = GetNode<TextureRect>("Ratio/IMG_Background");

		ActualCharacter = null;
		FirstCharacter = true;

		GCon.CurrentDialogue.ResetState();

		if (GCon.CurrentDayPart != GameController.EDayPart.Afternoon)
			IMG_Background.Texture = GCon.LocationsBackgrounds[GCon.CurrentLocation][0];
		else
			IMG_Background.Texture = GCon.LocationsBackgrounds[GCon.CurrentLocation][1];

		Continue();
	}

	public override void _Input(InputEvent @event)
	{
		if (GCon.IsGamePaused)
			return;

		base._Input(@event);

		if (@event.IsActionPressed("Next") && !PNL_Opts.Visible)
		{
			if (GCon.CurrentDialogue.CanContinue)
				Continue();
			else
			{
				ShowOptions();
				if (VBX_Opts.GetChildCount() == 0)
					End();
			}
		}
	}

	private string ProcessText(string text)
	{
		// Character detection
		if (text.Contains('<'))
		{
			int from = text.IndexOf("<") + "<".Length;
			int to = text.IndexOf(">");
			string name = text.Substring(from, to - from);

			ActualCharacter = GCon.Characters.Find(c => c.Name == name);
			if (ActualCharacter == null)
				GD.PushError("Character couldn't be found in GameController.");
			else
			{
				TXT_Name.Text = ActualCharacter.Name;
				IMG_Character.Texture = ActualCharacter.Emotions[(int)Character.EEmotions.Neutral];
			}

			text = text.Remove(from - 1, to + 1 - (from - 1));

			// Visits
			if (FirstCharacter && ActualCharacter != null)
			{
				GCon.Visits[ActualCharacter]++;
				FirstCharacter = false;
			}
		}

		// Expression detection
		if (text.Contains('['))
		{
			int from = text.IndexOf("[") + "[".Length;
			int to = text.IndexOf("]");
			string expression = text.Substring(from, to - from);

			switch (expression)
			{
				case "Neutral":
					IMG_Character.Texture =
						ActualCharacter.Emotions[(int)Character.EEmotions.Neutral];
					break;
				case "Happy":
					IMG_Character.Texture =
						ActualCharacter.Emotions[(int)Character.EEmotions.Happy];
					break;
				case "Sad":
					IMG_Character.Texture =
						ActualCharacter.Emotions[(int)Character.EEmotions.Sad];
					break;
			}

			text = text.Remove(from - 1, to + 1 - (from - 1));
		}

		// Variables
		// - %PLAYERNAME%
		if (text.Contains("%PLAYERNAME%"))
			text = text.Replace("%PLAYERNAME%", GCon.PlayerName);

		return text;
	}

	private void Continue(bool fromOption = false)
	{
		if (GCon.CurrentDialogue.CurrentChoices.Count <= 0)
		{
			if (fromOption)
				GCon.CurrentDialogue.Continue();

			if (GCon.CurrentDialogue.CanContinue)
				TXT_Dial.Text = ProcessText(GCon.CurrentDialogue.Continue());
			else
				End();
		}
	}

	private Button ProcessOption(InkChoice choice)
	{
		string text = choice.Text;
		Character.EReply reply = Character.EReply.Neutral;

		// Option detection
		if (text.Contains('<'))
		{
			int from = text.IndexOf("<") + "<".Length;
			int to = text.IndexOf(">");
			string option = text.Substring(from, to - from);

			switch (option)
			{
				case "G":
					reply = Character.EReply.Good;
					break;
				case "B":
					reply = Character.EReply.Bad;
					break;
			}

			text = text.Remove(from - 1, to + 1 - (from - 1));
		}

		Button button = new() { Text = text };
		button.Pressed += delegate
		{
			GCon.CurrentDialogue.ChooseChoiceIndex(choice.Index);
			Continue(true);
			PNL_Opts.Visible = false;

			switch (reply)
			{
				case Character.EReply.Neutral:
					break;
				case Character.EReply.Good:
					GCon.Points[ActualCharacter] += 1;
					break;
				case Character.EReply.Bad:
					GCon.Points[ActualCharacter] -= 1;
					break;
			}

			foreach (Node child in VBX_Opts.GetChildren())
				child.QueueFree();
		};

		return button;
	}

	private void ShowOptions()
	{
		foreach (InkChoice choice in GCon.CurrentDialogue.CurrentChoices)
		{
			VBX_Opts.AddChild(ProcessOption(choice));
			PNL_Opts.Visible = true;
		}
	}

	private void End()
	{
		GCon.Next();

		GetTree().ChangeSceneToFile("res://Level/Scenes/Map_P.tscn");
	}
}