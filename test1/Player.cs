using Godot;
using System;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Speed = 400; // How fast the player will move (pixels/sec).

    private Vector2 _screenSize; // Size of the game window.
    static private Random generator = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _screenSize = GetViewport().Size;

        // Slumpa fram en startposition
        Position = new Vector2(
            x: generator.Next(50, (int)_screenSize.x - 50),
            y: generator.Next(50, (int)_screenSize.y - 50)
        );
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Hastigheten
        var velocity = new Vector2();

        if (Input.IsActionPressed("ui_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            velocity.y -= 1;
        }

        // Välj rätt sprite
        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        // Spela upp spriteanimationer bara under förflyttning
        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }

        // Välj rätt animtionen beroende på vilket den förflyttar sig
        if (velocity.x != 0)
        {
            animatedSprite.Animation = "walk";
            animatedSprite.FlipV = false;
            animatedSprite.FlipH = velocity.x < 0;
        }
        else if (velocity.y != 0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = velocity.y > 0;
        }

        // Räkna ut nya positionen
        Position += velocity * delta;

        // Håll spelaren inanför scenen
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, _screenSize.x),
            y: Mathf.Clamp(Position.y, 0, _screenSize.y)
        );
    }

    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        Hide(); // Player disappears after being hit.
        EmitSignal("Hit");
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

}
