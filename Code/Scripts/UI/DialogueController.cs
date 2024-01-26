using Godot;
using GodotInk;

public partial class DialogueController : Control
{
	[Export] private InkStory story;
	private VBoxContainer vbx_text;
	private VBoxContainer vbx_opts;
	private Panel pnl_opts;

	public override void _Ready()
	{
		base._Ready();

		vbx_text = GetNode<VBoxContainer>("Text/VBoxContainer");
		vbx_opts = GetNode<VBoxContainer>("Options/VBoxContainer");
		pnl_opts = GetNode<Panel>("Options");

		Continue();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("Next") && !pnl_opts.Visible)
		{
			if (story.GetCanContinue())
				Continue();
			else
				ShowOptions();
		}
	}

	private void Continue()
	{
		if (story.CurrentChoices.Count <= 0)
		{
			foreach (Node child in vbx_text.GetChildren())
				child.QueueFree();

			Label content = new() { Text = story.Continue() };
			vbx_text.AddChild(content);
		}
	}

	private void ShowOptions()
	{
		foreach (Node child in vbx_opts.GetChildren())
			child.QueueFree();

		foreach (InkChoice choice in story.CurrentChoices)
		{
			Button button = new() { Text = choice.Text };
			button.Pressed += delegate
			{
				story.ChooseChoiceIndex(choice.Index);
				Continue();
				pnl_opts.Visible = false;
			};
			vbx_opts.AddChild(button);
			pnl_opts.Visible = true;
		}
	}
}
