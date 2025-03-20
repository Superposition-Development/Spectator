using Godot;
using System;

public partial class Boot : Control
{
    private VideoStreamPlayer player;
    public override void _Ready()
    {
        AspectRatioContainer centerContainer = GetNode<AspectRatioContainer>("AspectRatioContainer");
        player = centerContainer.GetNode<VideoStreamPlayer>("VideoStreamPlayer");
        player.Finished += LoadToMenu;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton)
        {
            LoadToMenu();
        }
    }

    private void LoadToMenu()
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/test.tscn");
        Node instance = scene.Instantiate();
        AddChild(instance);
        GetTree().ChangeSceneToFile("res://Scenes/test.tscn");
    }
}
