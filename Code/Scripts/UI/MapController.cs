using Godot;

public partial class MapController : Control
{
	private GameController GCon;

	private Control Buttons;
	private Button BTN_Hallway;
	private Button BTN_Library;
	private Button BTN_Gym;
	private Button BTN_Classroom;
	private Button BTN_Roof;
	private Button BTN_Lockers;
	private Button BTN_Map;
	private RichTextLabel TXT_Time;
	private TextureRect IMG_Map;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		Buttons = GetNode<Control>("IMG_Board/IMG_Map/Buttons");
		BTN_Hallway = GetNode<Button>("IMG_Board/IMG_Map/Buttons/BTN_Hallway");
		BTN_Library = GetNode<Button>("IMG_Board/IMG_Map/Buttons/BTN_Library");
		BTN_Gym = GetNode<Button>("IMG_Board/IMG_Map/Buttons/BTN_Gym");
		BTN_Classroom = GetNode<Button>("IMG_Board/IMG_Map/Buttons/BTN_Classroom");
		BTN_Roof = GetNode<Button>("IMG_Board/IMG_Map/Buttons/BTN_Roof");
		BTN_Lockers = GetNode<Button>("IMG_Board/IMG_Map/Buttons/BTN_Lockers");
		BTN_Map = GetNode<Button>("IMG_Board/IMG_Map/BTN_Map");
		TXT_Time = GetNode<RichTextLabel>("IMG_Board/TXT_Time");
		IMG_Map = GetNode<TextureRect>("IMG_Board/IMG_Map");

		BTN_Hallway.Pressed += delegate { GCon.GoTo(GameController.ELocation.Hallway); };
		BTN_Library.Pressed += delegate { GCon.GoTo(GameController.ELocation.Library); };
		BTN_Gym.Pressed += delegate { GCon.GoTo(GameController.ELocation.Gym); };
		BTN_Classroom.Pressed += delegate { GCon.GoTo(GameController.ELocation.Classroom); };
		BTN_Roof.Pressed += delegate { GCon.GoTo(GameController.ELocation.Roof); };
		BTN_Lockers.Pressed += delegate { GCon.GoTo(GameController.ELocation.Lockers); };
		BTN_Map.Pressed += delegate
		{
			IMG_Map.Scale = new Vector2(1, 1);
			Buttons.Visible = true;
		};

		switch (GCon.CurrentDayPart)
		{
			case GameController.EDayPart.Morning:
				TXT_Time.Text = "[right]Morning[/right]";
				break;
			case GameController.EDayPart.Midday:
				TXT_Time.Text = "[right]Midday[/right]";
				break;
			case GameController.EDayPart.Afternoon:
				TXT_Time.Text = "[right]Afternoon[/right]";
				break;
		}
	}
}
