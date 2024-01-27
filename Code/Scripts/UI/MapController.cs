using Godot;

public partial class MapController : Control
{
	private GameController GCon;

	private Button BTN_Hallway;
	private Button BTN_Library;
	private Button BTN_Gym;
	private Button BTN_Class;
	private Button BTN_Rooftop;
	private Button BTN_Entrance;
	private RichTextLabel TXT_Time;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GCon = GetNode<GameController>("/root/GameController");

		BTN_Hallway = GetNode<Button>("BTN_Hallway");
		BTN_Library = GetNode<Button>("BTN_Library");
		BTN_Gym = GetNode<Button>("BTN_Gym");
		BTN_Class = GetNode<Button>("BTN_Class");
		BTN_Rooftop = GetNode<Button>("BTN_Rooftop");
		BTN_Entrance = GetNode<Button>("BTN_Entrance");
		TXT_Time = GetNode<RichTextLabel>("TXT_Time");

		BTN_Hallway.Pressed += delegate { GCon.GoTo(GameController.ELocation.Hallway); };
		BTN_Library.Pressed += delegate { GCon.GoTo(GameController.ELocation.Library); };
		BTN_Gym.Pressed += delegate { GCon.GoTo(GameController.ELocation.Gym); };
		BTN_Class.Pressed += delegate { GCon.GoTo(GameController.ELocation.Class); };
		BTN_Rooftop.Pressed += delegate { GCon.GoTo(GameController.ELocation.Rooftop); };
		BTN_Entrance.Pressed += delegate { GCon.GoTo(GameController.ELocation.Entrance); };

		switch (GCon.CurrentDayPart)
		{
			case GameController.EDayPart.Morning:
				TXT_Time.Text = "[center]Morning[/center]";
				break;
			case GameController.EDayPart.Midday:
				TXT_Time.Text = "[center]Midday[/center]";
				break;
			case GameController.EDayPart.Afternoon:
				TXT_Time.Text = "[center]Afternoon[/center]";
				break;
		}
	}
}
