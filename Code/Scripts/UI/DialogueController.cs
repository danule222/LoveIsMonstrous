using System;
using Godot;
using GodotInk;

public partial class DialogueController : Control
{
	[Export] private InkStory story;
	private VBoxContainer vbx_text;
	private VBoxContainer vbx_opts;
	private Panel pnl_opts;
	private RichTextLabel lbl_name;

	public override void _Ready()
	{
		base._Ready();

		vbx_text = GetNode<VBoxContainer>("Text/VBoxContainer");
		vbx_opts = GetNode<VBoxContainer>("Options/VBoxContainer");
		pnl_opts = GetNode<Panel>("Options");
		lbl_name = GetNode<RichTextLabel>("Name/RichTextLabel");

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

			string label_content = story.Continue();
			if (label_content.Contains("<"))
			{
				int from = label_content.IndexOf("<") + "<".Length;
				int to = label_content.IndexOf(">");
				string name = label_content.Substring(from, to - from);

				lbl_name.Text = name;
				label_content = label_content.Remove(from - 1, (to + 1) - (from - 1));
			}

			Label content = new() { Text = label_content };
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
