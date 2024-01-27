using Godot;

public partial class MapController : Control
{
	public enum ELocation { Hallway, Library, Gym, Class, Rooftop, Entrance }

	private Button BTN_Hallway;
	private Button BTN_Library;
	private Button BTN_Gym;
	private Button BTN_Class;
	private Button BTN_Rooftop;
	private Button BTN_Entrance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BTN_Hallway = GetNode<Button>("BTN_Hallway");
		BTN_Library = GetNode<Button>("BTN_Library");
		BTN_Gym = GetNode<Button>("BTN_Gym");
		BTN_Class = GetNode<Button>("BTN_Class");
		BTN_Rooftop = GetNode<Button>("BTN_Rooftop");
		BTN_Entrance = GetNode<Button>("BTN_Entrance");

		BTN_Hallway.Pressed += delegate { GoTo(ELocation.Hallway); };
		BTN_Library.Pressed += delegate { GoTo(ELocation.Library); };
		BTN_Gym.Pressed += delegate { GoTo(ELocation.Gym); };
		BTN_Class.Pressed += delegate { GoTo(ELocation.Class); };
		BTN_Rooftop.Pressed += delegate { GoTo(ELocation.Rooftop); };
		BTN_Entrance.Pressed += delegate { GoTo(ELocation.Entrance); };
	}

	private void GoTo(ELocation location)
	{
		GD.Print(location);
	}
}
