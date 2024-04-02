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

	[Export]
	public NodePath Target { get; set; }

	AnimatedSprite2D animatedSprite;
	ShootAt shootAI;
	HoverChase hoverChase;
	private bool isShooting = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.AnimationFinished += AnimatedSprite_AnimationFinished;

		AddToGroup(HitGroups.Enemy);

		shootAI = new ShootAt(Shoot, ChangeDirection, 120);
		hoverChase = new HoverChase(this, 50, 300, 250);

		AddChild(shootAI);
		AddChild(hoverChase);

		Damageable.OnDeathCallback += OnDeath;
		Damageable.OnDamageCallback += OnDamage; 
	}

	private void OnDamage(int damage, Damageable damageable)
	{
		
	}
	private void OnDeath()
	{
		QueueFree();
	}
	private void AnimatedSprite_AnimationFinished()
	{
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
