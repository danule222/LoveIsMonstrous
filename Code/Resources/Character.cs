using Godot;
using GodotInk;

[GlobalClass]
public partial class Character : Resource
{
  private GameController GCon;

  public enum EEmotions { Neutral, Happy, Sad }
  public enum EReply { Neutral, Good, Bad }

  [Export] public string Name { get; set; }
  [Export] public Godot.Collections.Array<Texture2D> Emotions { get; set; } // Bruh
  [Export] public Godot.Collections.Array<GameController.ELocation> Timetable { get; set; }
  [Export] public Godot.Collections.Array<InkStory> Dialogues { get; set; }

  public Character() : this("", null) { }

  public Character(string name, Godot.Collections.Array<Texture2D> emotions)
  {
    Name = name;
    Emotions = emotions;
  }
}