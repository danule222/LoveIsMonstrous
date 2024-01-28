using Godot;
using GodotInk;
using System.Collections.Generic;

public partial class GameController : Node
{
	public enum EWeekDay { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
	public enum EDayPart { Morning, Midday, Afternoon }
	public enum ELocation { Hallway, Library, Gym, Classroom, Roof, Lockers }

	public List<Character> Characters = new List<Character>();
	public List<Character> CharactersVisitedToday = new List<Character>(2);
	public Dictionary<Character, int> Points = new Dictionary<Character, int>();
	public Dictionary<Character, int> Visits = new Dictionary<Character, int>();
	public Dictionary<Character, int> Stalkers = new Dictionary<Character, int>();
	public string PlayerName;
	public EWeekDay CurrentWeekDay;
	public EDayPart CurrentDayPart;
	public ELocation CurrentLocation;
	public InkStory CurrentDialogue;
	public bool IsGamePaused;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load characters
		Characters.Clear();
		Characters.Add(GD.Load<Character>("res://Characters/TestCharacter.tres"));

		// Set points
		Points.Clear();
		Visits.Clear();
		Stalkers.Clear();
		foreach (Character c in Characters)
		{
			Points.Add(c, 0);
			Visits.Add(c, 0);
			Stalkers.Add(c, 0);
		}

		PlayerName = "Jeremy Towers";
		CurrentWeekDay = EWeekDay.Monday;
		CurrentDayPart = EDayPart.Morning;
		CurrentLocation = ELocation.Hallway;
		CurrentDialogue = GD.Load<InkStory>("res://Ink/Test/Test.ink");

		IsGamePaused = false;
	}

	public void StartNewGame()
	{
		// Load characters
		Characters.Clear();
		Characters.Add(GD.Load<Character>("res://Characters/Azreial.tres"));
		Characters.Add(GD.Load<Character>("res://Characters/Eyeden.tres"));
		Characters.Add(GD.Load<Character>("res://Characters/Fern.tres"));
		Characters.Add(GD.Load<Character>("res://Characters/Gourdon.tres"));
		Characters.Add(GD.Load<Character>("res://Characters/Owlaf.tres"));
		Characters.Add(GD.Load<Character>("res://Characters/Wrenfield.tres"));

		// Set points
		Points.Clear();
		Visits.Clear();
		foreach (Character c in Characters)
		{
			Points.Add(c, 0);
			Visits.Add(c, 0);
		}

		PlayerName = "Jeremy Towers";
		CurrentWeekDay = EWeekDay.Monday;
		CurrentDayPart = EDayPart.Morning;
		CurrentLocation = ELocation.Hallway;
		CurrentDialogue = GD.Load<InkStory>("res://Ink/Test/Test.ink");

		IsGamePaused = false;

		GetTree().ChangeSceneToFile("res://Level/Scenes/Map_P.tscn");
	}

	public void GoTo(ELocation location)
	{
		Character c = Characters.Find(x => x.Timetable[(int)CurrentDayPart] == location);
		if (c == null) GD.PushError("There's no character assigned to "
		 														+ location + " at " + CurrentDayPart);
		else
		{
			// Check what dialogue is going to be used

			InkStory inkStory = null;

			if (CurrentDayPart == EDayPart.Afternoon)
			{
				if (CharactersVisitedToday[0] == CharactersVisitedToday[1])
				{
					int stalkerDialogueIndex = Stalkers[c];
					if (Stalkers[c] >= c.StalkerDialogues.Count)
						stalkerDialogueIndex = c.StalkerDialogues.Count - 1;

					inkStory = c.StalkerDialogues[stalkerDialogueIndex];
					Stalkers[c]++;
				}
				else
					inkStory = c.Dialogues[Visits[c]];
			}
			else
			{
				CharactersVisitedToday.Add(c);
				CurrentDialogue = c.Dialogues[Visits[c]];
			}

			if (inkStory != null)
				CurrentDialogue = inkStory;
		}

		GetTree().ChangeSceneToFile("res://Level/Scenes/Dialogue_P.tscn");
	}

	public void Next()
	{
		bool nextDay = false;

		if ((int)CurrentDayPart + 1 > (int)EDayPart.Afternoon)
		{
			nextDay = true;
			CurrentDayPart = EDayPart.Morning;
		}
		else
			CurrentDayPart++;

		if (nextDay)
		{
			if ((int)CurrentWeekDay + 1 >= (int)EWeekDay.Saturday)
				GD.Print("EndGame");
			else
				CurrentWeekDay++;
		}
	}
}