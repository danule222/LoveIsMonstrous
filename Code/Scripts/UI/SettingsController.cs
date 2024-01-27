using Godot;

public partial class SettingsController : Control
{
	public Panel PreviousMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (!Visible)
			return;

		base._Input(@event);

		if (@event.IsActionPressed("Pause"))
		{
			GoBack();
			GetViewport().SetInputAsHandled();
		}
	}

	private void GoBack()
	{
		Visible = false;
		PreviousMenu.Visible = true;
	}
}
