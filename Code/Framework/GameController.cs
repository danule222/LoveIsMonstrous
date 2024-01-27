using Godot;
using System.Collections.Generic;

public partial class GameController : Node
{
	public enum EWeekDay { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
	public enum EDayPart { Morning, Midday, Afternoon }

	static public List<Character> CHARACTERS = new List<Character>();
	static public Dictionary<Character, int> POINTS = new Dictionary<Character, int>();
	static public string PLAYER_NAME = "Jeremy Towers";
	static public EWeekDay ACTUAL_WEEK_DAY;
	static public EDayPart ACTUAL_DAY_PART;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load characters
		CHARACTERS.Add(GD.Load<Character>("res://Characters/TestCharacter.tres"));

		// Set points
		foreach (Character c in CHARACTERS)
			POINTS.Add(c, 0);

		ACTUAL_WEEK_DAY = EWeekDay.Monday;
		ACTUAL_DAY_PART = EDayPart.Morning;
	}
}