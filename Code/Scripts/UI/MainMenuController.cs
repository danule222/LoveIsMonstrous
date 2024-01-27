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

		BTN_Play = GetNode<Button>("VBoxContainer/BTN_Play");
		BTN_Settings = GetNode<Button>("VBoxContainer/BTN_Settings");
		BTN_Exit = GetNode<Button>("VBoxContainer/BTN_Exit");

		BTN_Play.Pressed += delegate { ClickedPlay(); };
		BTN_Settings.Pressed += delegate { ClickedSettings(); };
		BTN_Exit.Pressed += delegate { ClickedExit(); };
	}

	private void ClickedPlay()
	{
		// TODO: Start with prologue
		GetTree().ChangeSceneToFile("res://Level/Scenes/Map_P.tscn");
	}

	private void ClickedSettings() { }

	private void ClickedExit() { }
}
