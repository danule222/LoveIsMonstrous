using Godot;

public partial class PauseMenuController : Control
{
	private GameController GCon;

	private Button BTN_Continue;
	private Button BTN_Settings;
	private Button BTN_ToMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		BTN_Continue = GetNode<Button>("Panel/VBoxContainer/BTN_Continue");
		BTN_Settings = GetNode<Button>("Panel/VBoxContainer/BTN_Settings");
		BTN_ToMenu = GetNode<Button>("Panel/VBoxContainer/BTN_ToMenu");

		BTN_Continue.Pressed += delegate { ClickedContinue(); };
		BTN_Settings.Pressed += delegate { ClickedSettings(); };
		BTN_ToMenu.Pressed += delegate { ClickedToMenu(); };
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event.IsActionPressed("Pause"))
			TogglePauseMenu();
	}

	private void TogglePauseMenu()
	{
		Visible = !Visible;
		GCon.IsGamePaused = Visible;
	}

	private void ClickedContinue()
	{
		TogglePauseMenu();
	}

	private void ClickedSettings()
	{
		// TODO: Show settings
	}

	private void ClickedToMenu()
	{
		GetTree().ChangeSceneToFile("res://Level/Scenes/MainMenu_P.tscn");
		GCon.IsGamePaused = false;
	}
}
