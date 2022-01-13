using Godot;
using System;

public class Main : Node
{
    // Don't forget to rebuild the project so the editor knows about the new export variable.
    [Export]
    public PackedScene Mob;

    private int _score;

    // We use 'System.Random' as an alternative to GDScript's random methods.
    private Random _random = new Random();

    public override void _Ready()
    {
        NewGame();
    }

    // We'll use this later because C# doesn't support GDScript's randi().
    private float RandRange(float min, float max)
    {
        return (float)_random.NextDouble() * (max - min) + min;
    }

    public void GameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
    }

    public void NewGame()
    {
        _score = 0;

        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Position2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();
    }

    public void _on_StartTimer_timeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    public void _on_ScoreTimer_timeout()
    {
        _score++;
    }

    public void _on_MobTimer_timeout()
    {
        // Choose a random location on Path2D.
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.Offset = _random.Next();

        // Create a Mob instance and add it to the scene.
        var mobInstance = (RigidBody2D)Mob.Instance();
        AddChild(mobInstance);

        // Set the mob's direction perpendicular to the path direction.
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mobInstance.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;

        // Choose the velocity.
        mobInstance.LinearVelocity = new Vector2(RandRange(150f, 250f), 0).Rotated(direction);
    }
}
