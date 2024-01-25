using Godot;
using GodotInk;

public partial class DialogueController : Control
{
	[Export] private InkStory story;
	private VBoxContainer vbox;

	public override void _Ready()
	{
		vbox = GetNode<VBoxContainer>("Panel/VBoxContainer");
		ContinueStory();
	}

	private void ContinueStory()
	{
		foreach (Node child in vbox.GetChildren())
			child.QueueFree();

		Label content = new() { Text = story.ContinueMaximally() };
		vbox.AddChild(content);

		foreach (InkChoice choice in story.CurrentChoices)
		{
			Button button = new() { Text = choice.Text };
			button.Pressed += delegate
			{
				story.ChooseChoiceIndex(choice.Index);
				ContinueStory();
			};
			vbox.AddChild(button);
		}
	}
}
