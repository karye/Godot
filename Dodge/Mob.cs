using Godot;
using System;

public class Mob : RigidBody2D
{
    // Don't forget to rebuild the project so the editor knows about the new export variables.

    [Export]
    public int MinSpeed = 150; // Minimum speed range.

    [Export]
    public int MaxSpeed = 250; // Maximum speed range.

    // C# doesn't implement GDScript's random methods, so we use 'System.Random' as an alternative.
    static private Random _random = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        var mobTypes = animSprite.Frames.GetAnimationNames();
        animSprite.Animation = mobTypes[_random.Next(0, mobTypes.Length)];
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
