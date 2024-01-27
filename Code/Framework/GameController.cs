using Godot;
using System.Collections.Generic;

public partial class GameController : Node
{
	static public List<Character> Characters = new List<Character>();
	static public string PLAYER_NAME = "Jeremy Towers";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load characters
		Characters.Add(GD.Load<Character>("res://Characters/TestCharacter.tres"));
	}
}