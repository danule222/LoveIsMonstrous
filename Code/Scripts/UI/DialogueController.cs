using Godot;
using GodotInk;

public partial class DialogueController : Control
{
	[Export] private InkStory Story;
	private RichTextLabel TXT_Name;
	private RichTextLabel TXT_Dial;
	private VBoxContainer VBX_Opts;
	private Panel PNL_Opts;
	private TextureRect IMG_Character;
	private Character ActualCharacter;

	public override void _Ready()
	{
		base._Ready();

		TXT_Dial = GetNode<RichTextLabel>("Text/TXT_Dialogue");
		TXT_Name = GetNode<RichTextLabel>("Name/TXT_Name");
		VBX_Opts = GetNode<VBoxContainer>("Options/VBoxContainer");
		PNL_Opts = GetNode<Panel>("Options");
		IMG_Character = GetNode<TextureRect>("IMG_Character");

		ActualCharacter = null;

		Continue();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("Next") && !PNL_Opts.Visible)
		{
			if (Story.CanContinue)
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

			ActualCharacter = GameController.CHARACTERS.Find(c => c.Name == name);
			if (ActualCharacter == null)
				GD.PushError("Character couldn't be found in GameController.");
			else
				TXT_Name.Text = ActualCharacter.Name;

			text = text.Remove(from - 1, to + 1 - (from - 1));
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
						GameController.CHARACTERS[0].Emotions[(int)Character.EEmotions.Neutral];
					break;
				case "Happy":
					IMG_Character.Texture =
						GameController.CHARACTERS[0].Emotions[(int)Character.EEmotions.Happy];
					break;
				case "Sad":
					IMG_Character.Texture =
						GameController.CHARACTERS[0].Emotions[(int)Character.EEmotions.Sad];
					break;
			}

			text = text.Remove(from - 1, to + 1 - (from - 1));
		}

		// Variables
		// - %PLAYERNAME%
		if (text.Contains("%PLAYERNAME%"))
			text = text.Replace("%PLAYERNAME%", GameController.PLAYER_NAME);

		return text;
	}

	private void Continue(bool fromOption = false)
	{
		if (Story.CurrentChoices.Count <= 0)
		{
			if (fromOption)
				Story.Continue();

			if (Story.CanContinue)
				TXT_Dial.Text = ProcessText(Story.Continue());
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
			Story.ChooseChoiceIndex(choice.Index);
			Continue(true);
			PNL_Opts.Visible = false;

			ActualCharacter.SetPoints(reply);

			foreach (Node child in VBX_Opts.GetChildren())
				child.QueueFree();
		};

		return button;
	}

	private void ShowOptions()
	{
		foreach (InkChoice choice in Story.CurrentChoices)
		{
			VBX_Opts.AddChild(ProcessOption(choice));
			PNL_Opts.Visible = true;
		}
	}

	private void End()
	{
		// TODO: Implement End
		TXT_Dial.Text = "";
	}
}