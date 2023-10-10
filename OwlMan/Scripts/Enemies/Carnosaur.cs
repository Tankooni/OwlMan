using Atmo2.Enemy;
using Atmo2.Enemy.AI;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Carnosaur : Enemy
{
	[Export]
	public string AttackSoundName { get; set; }

	AnimatedSprite2D animatedSprite;
	ShootAt shootAI;
	private bool isShooting = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.AnimationFinished += AnimatedSprite_AnimationFinished;

		shootAI = new ShootAt(Shoot, ChangeDirection, 120)
		{
			TargetHitgroups = new List<string> { HitGroups.Player, HitGroups.Wall }
		};

		AddChild(shootAI);
		AddChild(new HoverChase(this, 150, 300, 250));
	}

	private void AnimatedSprite_AnimationFinished()
	{
		GD.Print("Carnosaur anim finished");
		if (isShooting)
		{
			animatedSprite.Play("idle");
			isShooting = false;
		}
	}

	public void Shoot()
	{
		isShooting = true;
		if (AttackSoundName != String.Empty)
			Overlord.OwlOverlord.PlaySound(AttackSoundName, this.GlobalPosition);
		animatedSprite.Play("attack");
	}

	public void ChangeDirection(Vector2 direction)
	{
		if (direction.X < 0)
			animatedSprite.FlipH = false;
		else if (direction.X > 0)
			animatedSprite.FlipH = true;
	}
}
