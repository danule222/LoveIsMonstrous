using Godot;

[GlobalClass]
public partial class Character : Resource
{
  public enum EEmotions { Neutral, Happy, Sad }
  public enum EReply { Neutral, Good, Bad }

  [Export] public string Name { get; set; }
  [Export] public Godot.Collections.Array<Texture2D> Emotions { get; set; } // Bruh

  public Character() : this("", null) { }

  public Character(string name, Godot.Collections.Array<Texture2D> emotions)
  {
    Name = name;
    Emotions = emotions;
  }

  public void SetPoints(EReply reply)
  {
    switch (reply)
    {
      case EReply.Neutral:
        break;
      case EReply.Good:
        GameController.POINTS[this] += 1;
        break;
      case EReply.Bad:
        GameController.POINTS[this] -= 1;
        break;
    }

    GD.Print(GameController.POINTS[this]);
  }
}