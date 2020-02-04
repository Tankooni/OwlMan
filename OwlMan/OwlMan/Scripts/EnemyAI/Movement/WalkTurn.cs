using Godot;
using System;

public class WalkTurn : Node2D
{
    public enum WalkingDirection : int
    {
        Left = -1,
        Right = 1
    }

    [Export]
    public WalkingDirection direction = WalkingDirection.Left;
    private Vector2 velocity = new Vector2(0,0);
    private int speed;
    private KinematicBody2D parent;

    private Action<int> changeDirection;

    public WalkTurn(Action<int> changeDirection, KinematicBody2D parent, int speed)
    {
        this.changeDirection = changeDirection;
        this.parent = parent;
        this.speed = speed;
    }
    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity.x = 0;
        velocity.y += Overlord.STANDARD_GRAVITY;
        if(parent.TestMove(parent.Transform, velocity)
            && ! parent.TestMove(parent.Transform, new Vector2(speed * 5 * (int)direction, velocity.y))
        )
        {
            direction = direction == WalkingDirection.Left ? WalkingDirection.Right : WalkingDirection.Left;
            changeDirection((int)direction);
        }
        velocity.x = speed * (int)direction;

        var collision = parent.MoveAndSlide(velocity);

        if(Math.Abs(collision.x) < speed)
        {
            direction = direction == WalkingDirection.Left ? WalkingDirection.Right : WalkingDirection.Left;
            changeDirection((int)direction);
        }
        if(collision.y == 0)
            velocity.y = 0;
    }
}
