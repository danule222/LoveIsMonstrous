using Godot;

public partial class MainMenuController : Control
{
	private GameController GCon;

	private VBoxContainer VBX_Buttons;
	private Button BTN_Play;
	private Button BTN_Settings;
	private Button BTN_Exit;
	private SettingsController SettingsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		VBX_Buttons = GetNode<VBoxContainer>("VBX_Buttons");
		BTN_Play = GetNode<Button>("VBX_Buttons/BTN_Play");
		BTN_Settings = GetNode<Button>("VBX_Buttons/BTN_Settings");
		BTN_Exit = GetNode<Button>("VBX_Buttons/BTN_Exit");

		SettingsMenu = GetNode<SettingsController>("SettingsMenu");
		SettingsMenu.PreviousMenu = VBX_Buttons;

		BTN_Play.Pressed += delegate { ClickedPlay(); };
		BTN_Settings.Pressed += delegate { ClickedSettings(); };
		BTN_Exit.Pressed += delegate { ClickedExit(); };
	}

	private void ClickedPlay()
	{
		// TODO: Start with prologue
		GCon.StartNewGame();
	}

	private void ClickedSettings()
	{
		VBX_Buttons.Visible = false;
		SettingsMenu.Visible = true;
	}

	private void ClickedExit()
	{
		GetTree().Quit();
	}
}
