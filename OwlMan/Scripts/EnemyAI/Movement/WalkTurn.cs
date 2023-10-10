using Godot;
using System;

public partial class WalkTurn : Node2D
{
	public enum WalkingDirection : int
	{
		Left = -1,
		Right = 1
	}

	[Export]
	public WalkingDirection direction = WalkingDirection.Left;
	private Vector2 velocity = new Vector2(0, 0);
	private int speed;
	private CharacterBody2D parent;

	private Action<int> changeDirection;

	public WalkTurn(Action<int> changeDirection, CharacterBody2D parent, int speed)
	{
		this.changeDirection = changeDirection;
		this.parent = parent;
		this.speed = speed;
	}
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity.X = 0;
		velocity.Y += Overlord.STANDARD_GRAVITY;
		if (parent.TestMove(parent.Transform, velocity)
			&& !parent.TestMove(parent.Transform, new Vector2(speed * 5 * (int)direction, velocity.Y))
		)
		{
			direction = direction == WalkingDirection.Left ? WalkingDirection.Right : WalkingDirection.Left;
			changeDirection((int)direction);
		}
		velocity.X = speed * (int)direction;
		parent.Velocity = velocity;
		var hasCollided = parent.MoveAndSlide();
		if(hasCollided)
		{
			//for (int i = 0; i < parent.GetSlideCollisionCount(); i++)
			//{
			//	var collision = parent.GetSlideCollision(i);

			//	GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
			//}
			direction = direction == WalkingDirection.Left ? WalkingDirection.Right : WalkingDirection.Left;
			changeDirection((int)direction);
		}

		//var collision = parent.MoveAndCollide(parent.Velocity);

		//if (Math.Abs(collision.X) < speed)
		//{
		//	direction = direction == WalkingDirection.Left ? WalkingDirection.Right : WalkingDirection.Left;
		//	changeDirection((int)direction);
		//}
		//if (collision.Y == 0)
		//	velocity.Y = 0;
	}
}
