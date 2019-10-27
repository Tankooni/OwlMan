using Atmo2;
using Godot;
using System;

public class Bug : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AnimatedSprite image;

	private float VelX = 0;
	private float VelY = 0;
	private bool walkingLeft = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		image = GetNode<AnimatedSprite>("AnimatedSprite");
		image.Play("run");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		walkingLeft = false;
		if (walkingLeft)
		{
			VelX = -200;
			image.SetFlipH(!walkingLeft);
		}
		else
		{
			VelX = 200;
			image.SetFlipH(!walkingLeft);
		}
		VelY += KQ.STANDARD_GRAVITY;
		var velocity = new Vector2(VelX, VelY);
		var collision = MoveAndSlide(velocity);

		//if (collision != null)
		//{
		//	GD.Print(velocity, collision);
		//	//velocity = velocity.Slide(collision.Normal);

		//	if (collision.x != 0)
		//	{
		//		walkingLeft = !walkingLeft;
		//	}
		//}
	}
}
