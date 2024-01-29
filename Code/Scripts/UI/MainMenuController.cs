using Godot;

public partial class MainMenuController : Control
{
	private GameController GCon;

	private VBoxContainer VBX_Buttons;
	private Button BTN_Play;
	private Button BTN_Settings;
	private Button BTN_Credits;
	private Button BTN_Exit;
	private SettingsController SettingsMenu;
	private CreditsController CreditsMenu;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		VBX_Buttons = GetNode<VBoxContainer>("Column");
		BTN_Play = GetNode<Button>("Column/Buttons/BTN_Play");
		BTN_Settings = GetNode<Button>("Column/Buttons/VBoxContainer/BTN_Settings");
		BTN_Credits = GetNode<Button>("Column/Buttons/VBoxContainer/BTN_Credits");
		BTN_Exit = GetNode<Button>("Column/Buttons/VBoxContainer/BTN_Exit");

		SettingsMenu = GetNode<SettingsController>("SettingsMenu");
		SettingsMenu.PreviousMenu = VBX_Buttons;
		CreditsMenu = GetNode<CreditsController>("CreditsMenu");
		CreditsMenu.PreviousMenu = VBX_Buttons;

		BTN_Play.Pressed += delegate { ClickedPlay(); };
		BTN_Settings.Pressed += delegate { ClickedSettings(); };
		BTN_Credits.Pressed += delegate { ClickedCredits(); };
		BTN_Exit.Pressed += delegate { ClickedExit(); };

		GCon.MusicPlayer.Stream = GD.Load<AudioStream>("res://Audio/Music/A_MainMenu.wav");
		GCon.MusicPlayer.Play();
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

	private void ClickedCredits()
	{
		VBX_Buttons.Visible = false;
		CreditsMenu.Visible = true;
	}

	private void ClickedExit()
	{
		GetTree().Quit();
	}
}
