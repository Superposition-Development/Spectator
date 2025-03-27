using Godot;

public partial class Boot : Control
{
    private VideoStreamPlayer player;
    private int audioBusIdx;
    private AspectRatioContainer centerContainer;

    public override void _Ready()
    {
        Tween fadeIn = CreateTween().SetParallel();
        
        audioBusIdx = AudioServer.GetBusIndex("Boot");

        // Rename m8
        centerContainer = GetNode<AspectRatioContainer>("AspectRatioContainer");
        
        centerContainer.Modulate = new Color("TRANSPARENT");

        fadeIn.TweenProperty(centerContainer, "modulate", new Color("WHITE"), 1.0);
        fadeIn.TweenMethod(Callable.From((float target) => SetBusVolume(audioBusIdx, target)), 0.0f, 1.0f, 1.0);

        player = centerContainer.GetNode<VideoStreamPlayer>("VideoStreamPlayer");
        player.Finished += LoadToMenu;
    }

    // TODO: Move this to an autoload for settings management
    private static void SetBusVolume(int idx, float volume) {
        AudioServer.SetBusVolumeLinear(idx, volume);
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton)
        {
            LoadToMenu();
        }
    }

    private async void LoadToMenu()
    {
        // Fyi, I'm gonna make a custom scene loader probably

        Tween fadeOut = CreateTween().SetParallel();
        fadeOut.TweenProperty(centerContainer, "modulate", new Color("TRANSPARENT"), 1.0);
        fadeOut.TweenMethod(Callable.From((float target) => SetBusVolume(audioBusIdx, target)), 1.0f, 0.0f, 1.0);

        await ToSignal(fadeOut, Tween.SignalName.Finished);

        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/test.tscn");
        Node instance = scene.Instantiate();
        AddChild(instance);
        GetTree().ChangeSceneToFile("res://Scenes/test.tscn");
    }
}
