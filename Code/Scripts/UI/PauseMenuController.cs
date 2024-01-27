using Godot;

public partial class PauseMenuController : Control
{
	private GameController GCon;

	private Panel PNL_PauseMenu;
	private Button BTN_Continue;
	private Button BTN_Settings;
	private Button BTN_ToMenu;
	private SettingsController SettingsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		PNL_PauseMenu = GetNode<Panel>("PNL_PauseMenu");
		BTN_Continue = GetNode<Button>("PNL_PauseMenu/VBoxContainer/BTN_Continue");
		BTN_Settings = GetNode<Button>("PNL_PauseMenu/VBoxContainer/BTN_Settings");
		BTN_ToMenu = GetNode<Button>("PNL_PauseMenu/VBoxContainer/BTN_ToMenu");

		SettingsMenu = GetNode<SettingsController>("SettingsMenu");
		SettingsMenu.PreviousMenu = PNL_PauseMenu;

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
		PNL_PauseMenu.Visible = false;
		SettingsMenu.Visible = true;
	}

	private void ClickedToMenu()
	{
		GetTree().ChangeSceneToFile("res://Level/Scenes/MainMenu_P.tscn");
		GCon.IsGamePaused = false;
	}
}
