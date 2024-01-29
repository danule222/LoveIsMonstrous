using Godot;
using GodotInk;
using System.Collections.Generic;
using System.Collections.Specialized;

public partial class GameController : Node
{
	public enum EWeekDay { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
	public enum EDayPart { Morning, Midday, Afternoon }
	public enum ELocation { Hallway, Library, Gym, Classroom, Rooftop, Lockers }
	public enum EEndGame { Lover, Friend, Solo, Stalker }

	public List<Character> Characters = new List<Character>();
	public List<Character> CharactersVisitedToday = new List<Character>(2);
	public List<Character> EndGameCharacters = new List<Character>();
	public Dictionary<Character, int> Points = new Dictionary<Character, int>();
	public Dictionary<Character, int> Visits = new Dictionary<Character, int>();
	public Dictionary<Character, int> Stalkers = new Dictionary<Character, int>();
	public Dictionary<ELocation, List<Texture2D>> LocationsBackgrounds =
		new Dictionary<ELocation, List<Texture2D>>();
	public string PlayerName;
	public EWeekDay CurrentWeekDay;
	public EDayPart CurrentDayPart;
	public ELocation CurrentLocation;
	public EEndGame EndType;
	public InkStory CurrentDialogue;
	public bool IsGamePaused;
	public AudioStreamPlayer MusicPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
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

		string backPath = "res://Art/Textures/Backgrounds/";
		LocationsBackgrounds.Clear();
		LocationsBackgrounds.Add(
			ELocation.Classroom,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Classroom_Morning.png"), GD.Load<Texture2D>(backPath + "T_Classroom_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Gym,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Gym_Morning.png"), GD.Load<Texture2D>(backPath + "T_Gym_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Hallway,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Hallway_Morning.png"), GD.Load<Texture2D>(backPath + "T_Hallway_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Rooftop,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Rooftop_Morning.png"), GD.Load<Texture2D>(backPath + "T_Rooftop_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Library,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Library_Morning.png"), GD.Load<Texture2D>(backPath + "T_Library_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Lockers,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Lockers_Morning.png"), GD.Load<Texture2D>(backPath + "T_Lockers_Afternoon.png") }
		);

		// Game music
		MusicPlayer = new AudioStreamPlayer
		{
			Bus = "Music"
		};
		AddChild(MusicPlayer);

		IsGamePaused = false;
	}

	public void StartNewGame()
	{
		// Load characters
		Characters.Clear();
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

		string backPath = "res://Art/Textures/Backgrounds/";
		LocationsBackgrounds.Clear();
		LocationsBackgrounds.Add(
			ELocation.Classroom,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Classroom_Morning.png"), GD.Load<Texture2D>(backPath + "T_Classroom_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Gym,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Gym_Morning.png"), GD.Load<Texture2D>(backPath + "T_Gym_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Hallway,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Hallway_Morning.png"), GD.Load<Texture2D>(backPath + "T_Hallway_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Rooftop,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Rooftop_Morning.png"), GD.Load<Texture2D>(backPath + "T_Rooftop_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Library,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Library_Morning.png"), GD.Load<Texture2D>(backPath + "T_Library_Afternoon.png") }
		);
		LocationsBackgrounds.Add(
			ELocation.Lockers,
			new List<Texture2D>() { GD.Load<Texture2D>(backPath + "T_Lockers_Morning.png"), GD.Load<Texture2D>(backPath + "T_Lockers_Afternoon.png") }
		);

		IsGamePaused = false;
		MusicPlayer.Stream = GD.Load<AudioStream>("res://Audio/Music/A_DayToDayWithMelodyLoop.wav");
		MusicPlayer.Play();

		GetTree().ChangeSceneToFile("res://Level/Scenes/Map_P.tscn");
	}

	public void GoTo(ELocation location)
	{
		CurrentLocation = location;

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
			{
				CurrentWeekDay++;
				EndGame();
			}
			else
				CurrentWeekDay++;
		}

		if (CurrentWeekDay == EWeekDay.Wednesday && CurrentDayPart == EDayPart.Morning)
		{
			MusicPlayer.Stream = GD.Load<AudioStream>("res://Audio/Music/A_DayToDay2.wav");
			MusicPlayer.Play();
		}
	}

	private void EndGame()
	{
		List<Character> toRomance = new List<Character>();
		List<Character> toFriend = new List<Character>();

		foreach (var item in Points)
		{
			if (item.Value > 7)
			{
				toRomance.Add(item.Key);
				continue;
			}
			else if (item.Value > 4)
			{
				toFriend.Add(item.Key);
				continue;
			}
		}

		// Choose ending
		if (toRomance.Count == 0 && toFriend.Count == 0)
		{
			EndType = EEndGame.Solo;
		}
		else if (toRomance.Count > 0)
		{
			EndType = EEndGame.Lover;
			EndGameCharacters.AddRange(toRomance);
		}
		else
		{
			EndType = EEndGame.Friend;
			EndGameCharacters.AddRange(toFriend);
		}

		GetTree().ChangeSceneToFile("res://Level/Scenes/EndDialogue_P.tscn");
	}
}