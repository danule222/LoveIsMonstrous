using Godot;
using GodotInk;

public partial class DialogueController : Control
{
	[Export] private InkStory Story;
	private RichTextLabel TXT_Name;
	private RichTextLabel TXT_Dial;
	private VBoxContainer VBX_Dial;
	private Panel PNL_Opts;
	private TextureRect IMG_Character;
	private Character ActualCharacter;

	public override void _Ready()
	{
		base._Ready();

		TXT_Dial = GetNode<RichTextLabel>("Text/TXT_Dialogue");
		TXT_Name = GetNode<RichTextLabel>("Name/TXT_Name");
		VBX_Dial = GetNode<VBoxContainer>("Options/VBoxContainer");
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
			if (Story.GetCanContinue())
				Continue();
			else
				ShowOptions();
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

			GD.Print(name);
			ActualCharacter = GameController.Characters.Find(c => c.Name == name);
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
						GameController.Characters[0].Emotions[(int)Character.EEmotions.Neutral];
					break;
				case "Happy":
					IMG_Character.Texture =
						GameController.Characters[0].Emotions[(int)Character.EEmotions.Happy];
					break;
				case "Sad":
					IMG_Character.Texture =
						GameController.Characters[0].Emotions[(int)Character.EEmotions.Sad];
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

	private void Continue()
	{
		if (Story.CurrentChoices.Count <= 0)
			TXT_Dial.Text = ProcessText(Story.Continue());
	}

	private void ShowOptions()
	{
		foreach (Node child in VBX_Dial.GetChildren())
			child.QueueFree();

		foreach (InkChoice choice in Story.CurrentChoices)
		{
			Button button = new() { Text = choice.Text };
			button.Pressed += delegate
			{
				Story.ChooseChoiceIndex(choice.Index);
				Continue();
				PNL_Opts.Visible = false;
			};
			VBX_Dial.AddChild(button);
			PNL_Opts.Visible = true;
		}
	}
}