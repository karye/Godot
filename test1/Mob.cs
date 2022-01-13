using Godot;
using System;

public class Mob : RigidBody2D
{
    [Export]
    public int MinSpeed = 150; // Minimum speed range.

    [Export]
    public int MaxSpeed = 250; // Maximum speed range.

    static private Random generator = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var animSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        // Hur många apriteanimationer har vi?
        var mobTypes = animSprite.Frames.GetAnimationNames();

        // Välj en slumpmässigt
        animSprite.Animation = mobTypes[generator.Next(0, mobTypes.Length)];
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
