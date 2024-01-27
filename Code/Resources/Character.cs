using Godot;

[GlobalClass]
public partial class Character : Resource
{
  public enum EEmotions { Neutral, Happy, Sad }

  [Export] public string Name { get; set; }
  [Export] public Godot.Collections.Array<Texture2D> Emotions { get; set; } // Bruh

  public Character() : this("", null) { }

  public Character(string name, Godot.Collections.Array<Texture2D> emotions)
  {
    Name = name;
    Emotions = emotions;
  }
}