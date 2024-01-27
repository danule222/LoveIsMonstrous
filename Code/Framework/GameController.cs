using Godot;
using System.Collections.Generic;

public partial class GameController : Node
{
	static public List<Character> CHARACTERS = new List<Character>();
	static public Dictionary<Character, int> POINTS = new Dictionary<Character, int>();
	static public string PLAYER_NAME = "Jeremy Towers";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load characters
		CHARACTERS.Add(GD.Load<Character>("res://Characters/TestCharacter.tres"));

		// Set points
		foreach (Character c in CHARACTERS)
			POINTS.Add(c, 0);
	}
}