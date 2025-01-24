using Atmo2.Enemy;
using Atmo2.Enemy.AI;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FlyChaser : Enemy
{
	[Export]
	public string AttackSoundName { get; set; }

	[Export]
	public NodePath Target { get; set; }
	ShootAt shootAI;
	ChaseDown chaseDown;
	private bool isShooting = false;
	private bool isMovingTracked = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		// Sprite2D_Old.AnimationFinished += AnimatedSprite_AnimationFinished;

		AddToGroup(HitGroups.Enemy);

		chaseDown = new ChaseDown(this, 100, 300);

		AddChild(chaseDown);

		AnimPlayer.Play("Idle");
	}

	private void AnimatedSprite_AnimationFinished()
	{
		if (isShooting)
		{
			AnimPlayer.Play("Idle");
			isShooting = false;
		}
	}

	public void ChangeDirection(Vector2 direction)
	{
		if (direction.X < 0)
			Sprite.FlipH = false;
		else if (direction.X > 0)
			Sprite.FlipH = true;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		ChangeDirection(Velocity);

		if( Velocity.Length() > 0 )
		{
			if( !isMovingTracked )
			{
				isMovingTracked = true;
				AnimPlayer.Play("Chase");
			}
		}
		else
		{	
			if( isMovingTracked )
			{
				isMovingTracked = false;
				AnimPlayer.Play("Idle");
			}
		}
    }
}
