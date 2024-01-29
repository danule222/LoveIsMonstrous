using Godot;

public partial class SettingsController : Control
{
	public Control PreviousMenu;

	private Slider SLI_Music;
	private Slider SLI_Effects;
	private Button BTN_Back;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SLI_Music = GetNode<Slider>("Panel/VBoxContainer/MusicSlider/SLI_Music");
		SLI_Effects = GetNode<Slider>("Panel/VBoxContainer/EffectsSlider/SLI_Effects");
		BTN_Back = GetNode<Button>("Panel/BTN_Back");

		SLI_Music.Value = DecibelToLinear(AudioServer.GetBusVolumeDb(1));
		SLI_Music.ValueChanged += delegate
		{
			AudioServer.SetBusVolumeDb(1, LinearToDecibel((float)SLI_Music.Value));
		};
		SLI_Effects.Value = DecibelToLinear(AudioServer.GetBusVolumeDb(2));
		SLI_Effects.ValueChanged += delegate
		{
			AudioServer.SetBusVolumeDb(2, LinearToDecibel((float)SLI_Effects.Value));
		};

		BTN_Back.Pressed += delegate { GoBack(); };
	}

	public override void _Input(InputEvent @event)
	{
		if (!Visible)
			return;

		base._Input(@event);

		if (@event.IsActionPressed("Pause"))
		{
			GoBack();
			GetViewport().SetInputAsHandled();
		}
	}

	private void GoBack()
	{
		Visible = false;
		PreviousMenu.Visible = true;
	}

	private float LinearToDecibel(float linear)
	{
		float dB;
		if (linear != 0)
			dB = 20.0f * Mathf.Log(linear);
		else
			dB = -144.0f;
		return dB;
	}

	private float DecibelToLinear(float dB)
	{
		float linear = Mathf.Pow(10.0f, dB / 20.0f);
		return linear;
	}
}
