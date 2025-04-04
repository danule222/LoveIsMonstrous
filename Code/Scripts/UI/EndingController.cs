using Godot;
using GodotInk;

public partial class EndingController : Control
{
  private GameController GCon;

  private RichTextLabel TXT_Name;
  private RichTextLabel TXT_Dial;
  private VBoxContainer VBX_Opts;
  private Panel PNL_Opts;
  private TextureRect IMG_Character;
  private TextureRect IMG_Background;
  private Character ActualCharacter;
  private InkStory ActualDialogue;

  [Export] public Godot.Collections.Array<Texture2D> EndBackgrounds;
  [Export] public Godot.Collections.Array<AudioStream> EndMusic;

  public override void _Ready()
  {
    GCon = GetNode<GameController>("/root/GameController");

    TXT_Dial = GetNode<RichTextLabel>("Text/TXT_Dialogue");
    TXT_Name = GetNode<RichTextLabel>("Name/TXT_Name");
    VBX_Opts = GetNode<VBoxContainer>("Options/VBoxContainer");
    PNL_Opts = GetNode<Panel>("Options");
    IMG_Character = GetNode<TextureRect>("IMG_Character");
    IMG_Background = GetNode<TextureRect>("Ratio/IMG_Background");

    // Check end
    switch (GCon.EndType)
    {
      case GameController.EEndGame.Lover:
        IMG_Background.Texture = EndBackgrounds[0];
        TXT_Dial.Text = "Select who you want to spend a special evening with.";
        GCon.MusicPlayer.Stream = EndMusic[0];
        break;
      case GameController.EEndGame.Friend:
        IMG_Background.Texture = EndBackgrounds[1];
        TXT_Dial.Text = "Select who you want to have fun with.";
        GCon.MusicPlayer.Stream = EndMusic[0];
        break;
      case GameController.EEndGame.Solo:
        IMG_Background.Texture = EndBackgrounds[2];
        TXT_Dial.Text = "Our protagonist was unable to make friends with any of his " +
          "companions and was left alone at home, watching the fireworks from the solitude of his house. FIN";
        GCon.MusicPlayer.Stream = EndMusic[1];
        break;
      case GameController.EEndGame.Stalker:
        IMG_Background.Texture = EndBackgrounds[3];
        TXT_Dial.Text = "Our protagonist, after stalking one of his colleagues, got what he deserved. FIN.";
        GCon.MusicPlayer.Stream = EndMusic[1];
        break;
    }
    TXT_Name.Text = "";
    GCon.MusicPlayer.Play();

    // Add character options
    if (GCon.EndType != GameController.EEndGame.Solo
        && GCon.EndType != GameController.EEndGame.Stalker)
    {
      foreach (var c in GCon.EndGameCharacters)
      {
        Button button = new() { Text = c.Name };
        button.Pressed += delegate
        {
          ActualCharacter = c;
          PNL_Opts.Visible = false;

          if (GCon.EndType == GameController.EEndGame.Lover)
            ActualDialogue = c.EndGameDialogues[1];
          else if (GCon.EndType == GameController.EEndGame.Friend)
            ActualDialogue = c.EndGameDialogues[2];

          IMG_Character.Visible = true;
          IMG_Character.Texture = c.Emotions[0];

          Continue();

          foreach (Node child in VBX_Opts.GetChildren())
            child.QueueFree();
        };
        VBX_Opts.AddChild(button);
      }
    }

    if (VBX_Opts.GetChildCount() > 0)
    {
      PNL_Opts.Visible = true;
    }
  }

  public override void _Input(InputEvent @event)
  {
    if (GCon.IsGamePaused)
      return;

    base._Input(@event);

    if (@event.IsActionPressed("Next") && !PNL_Opts.Visible)
    {
      if (GCon.EndType == GameController.EEndGame.Solo ||
        GCon.EndType == GameController.EEndGame.Stalker)
      {
        End();
        return;
      }

      if (ActualDialogue.CanContinue)
        Continue();
      else
      {
        ShowOptions();
        if (VBX_Opts.GetChildCount() == 0)
          End();
      }
    }
  }

  private string ProcessText(string text)
  {
    // Character detection
    if (text.Contains('<'))
    {
      int from = text.IndexOf("<") + "<".Length;
      int to = text.IndexOf(">");
      string name = text.Substring(from, to - from);

      ActualCharacter = GCon.Characters.Find(c => c.Name == name);
      if (ActualCharacter == null)
        GD.PushError("Character couldn't be found in GameController.");
      else
      {
        TXT_Name.Text = ActualCharacter.Name;
        IMG_Character.Texture = ActualCharacter.Emotions[(int)Character.EEmotions.Neutral];
      }

      text = text.Remove(from - 1, to + 1 - (from - 1));
    }

    // Expression detection
    if (text.Contains('['))
    {
      int from = text.IndexOf("[") + "[".Length;
      int to = text.IndexOf("]");
      string expression = text.Substring(from, to - from);

      switch (expression)
      {
        case "Neutral":
          IMG_Character.Texture =
            ActualCharacter.Emotions[(int)Character.EEmotions.Neutral];
          break;
        case "Happy":
          IMG_Character.Texture =
            ActualCharacter.Emotions[(int)Character.EEmotions.Happy];
          break;
        case "Sad":
          IMG_Character.Texture =
            ActualCharacter.Emotions[(int)Character.EEmotions.Sad];
          break;
      }

      text = text.Remove(from - 1, to + 1 - (from - 1));
    }

    // Variables
    // - %PLAYERNAME%
    if (text.Contains("%PLAYERNAME%"))
      text = text.Replace("%PLAYERNAME%", GCon.PlayerName);

    return text;
  }

  private void Continue(bool fromOption = false)
  {
    if (ActualDialogue.CurrentChoices.Count <= 0)
    {
      if (fromOption)
        ActualDialogue.Continue();

      if (ActualDialogue.CanContinue)
        TXT_Dial.Text = ProcessText(ActualDialogue.Continue());
      else
        End();
    }
  }

  private Button ProcessOption(InkChoice choice)
  {
    string text = choice.Text;
    Character.EReply reply = Character.EReply.Neutral;

    // Option detection
    if (text.Contains('<'))
    {
      int from = text.IndexOf("<") + "<".Length;
      int to = text.IndexOf(">");
      string option = text.Substring(from, to - from);

      switch (option)
      {
        case "G":
          reply = Character.EReply.Good;
          break;
        case "B":
          reply = Character.EReply.Bad;
          break;
      }

      text = text.Remove(from - 1, to + 1 - (from - 1));
    }

    Button button = new() { Text = text };
    button.Pressed += delegate
    {
      ActualDialogue.ChooseChoiceIndex(choice.Index);
      Continue(true);
      PNL_Opts.Visible = false;

      switch (reply)
      {
        case Character.EReply.Neutral:
          break;
        case Character.EReply.Good:
          GCon.Points[ActualCharacter] += 1;
          break;
        case Character.EReply.Bad:
          GCon.Points[ActualCharacter] -= 1;
          break;
      }

      foreach (Node child in VBX_Opts.GetChildren())
        child.QueueFree();
    };

    return button;
  }

  private void ShowOptions()
  {
    foreach (InkChoice choice in ActualDialogue.CurrentChoices)
    {
      VBX_Opts.AddChild(ProcessOption(choice));
      PNL_Opts.Visible = true;
    }
  }

  private void End()
  {
    GetTree().ChangeSceneToFile("res://Level/Scenes/MainMenu_P.tscn");
  }
}