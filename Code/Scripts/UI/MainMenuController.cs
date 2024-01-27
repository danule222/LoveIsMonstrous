using Godot;

public partial class MainMenuController : Control
{
	private GameController GCon;

	private Button BTN_Play;
	private Button BTN_Settings;
	private Button BTN_Exit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		BTN_Play = GetNode<Button>("Buttons/BTN_Play");
		BTN_Settings = GetNode<Button>("Buttons/BTN_Settings");
		BTN_Exit = GetNode<Button>("Buttons/BTN_Exit");

		BTN_Play.Pressed += delegate { ClickedPlay(); };
		BTN_Settings.Pressed += delegate { ClickedSettings(); };
		BTN_Exit.Pressed += delegate { ClickedExit(); };
	}

	private void ClickedPlay()
	{
		// TODO: Start with prologue
		GCon.StartNewGame();
	}

	private void ClickedSettings() { }

	private void ClickedExit()
	{
		GetTree().Quit();
	}
}
