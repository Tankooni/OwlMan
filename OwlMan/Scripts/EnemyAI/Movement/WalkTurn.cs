using Godot;
using System;

public partial class WalkTurn : Node
{
	public enum WalkingDirection : int
	{
		Left = -1,
		Right = 1
	}

	[Export]
	public WalkingDirection direction = WalkingDirection.Left;
	[Export]
	public int Speed = 200;
	[Export]
	private RayCast2D leftCast;
	[Export]
	private RayCast2D rightCast;
	private Vector2 velocity = new Vector2(0, 0);
	private Vector2 currentFloorNormal = new Vector2(0, 0);

	private CharacterBody2D parent;

	public Action<int> changeDirection;

	public override void _Ready()
	{
		parent = Owner as CharacterBody2D;
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity.X = 0;
		// If we're on the floor, we want to reset out Y velocity
		if( parent.IsOnFloor() ) 
		{
			velocity.Y = 0;
		}
		velocity.Y += Overlord.STANDARD_GRAVITY;

		velocity.X = Speed * (int)direction;
		parent.Velocity = velocity;
		
		if( parent.MoveAndSlide() )
		{
			velocity.Y = 0;
		}

		if( parent.IsOnWall() )
		{
			// var wallNormal = parent.GetWallNormal();
			// if(Mathf.Sign( parent.GetWallNormal().X ) < 0)
			// {
				
			// }
			FlipFlop();
			return;
		}
		
		if(parent.IsOnFloor())
		{
			switch (direction)
			{
				default:
				case WalkingDirection.Left:
					if( !leftCast.IsColliding() )
					{
						FlipFlop( WalkingDirection.Right );
						return;
					}
					break;
				case WalkingDirection.Right:
					if( !rightCast.IsColliding() )
					{
						FlipFlop( WalkingDirection.Left );
						return;
					}
					break;
			}
		}
	}


	private void FlipFlop()
	{
		FlipFlop( Mathf.Sign( parent.GetWallNormal().X ) < 0 ? WalkingDirection.Left : WalkingDirection.Right);
	}
	private void FlipFlop(WalkingDirection newDirection)
	{
		direction = newDirection;
		changeDirection((int)direction);
	}
}
