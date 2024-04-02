using Godot;
using Godot.Collections;
using Atmo2.Enemy.AI;
using System.Collections.Generic;
using System.Linq;

namespace Atmo2.Enemy
{
	public partial class Bee : Enemy
	{
		[Export]
		public string AttackSoundName { get; set; }

		private ShootAt shootAI;

		AnimatedSprite2D animatedSprite;

		private bool isShooting = false;

		public override void _Ready()
		{
			base._Ready();

			animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite.AnimationFinished += AnimatedSprite_AnimationFinished;

			animatedSprite.Play("idle");

			AddToGroup(HitGroups.Enemy);

			AddChild(shootAI = new ShootAt(Shoot, ChangeDirection, 60));

			Damageable.OnDeathCallback += OnDeath;
			Damageable.OnDamageCallback += OnDamage; 
		}
		private void OnDamage(int damage, Damageable damageable)
		{
			Velocity = new Vector2(100, 100);
			MoveAndSlide();
			Velocity = Vector2.Zero;
		}
		private void OnDeath()
		{
			LootDropCalculation();
		}

		private void AnimatedSprite_AnimationFinished()
		{
			if(isShooting)
			{
				animatedSprite.Play("idle");
				isShooting = false;
			}
		}

		public void Shoot()
		{
			isShooting = true;
			if (AttackSoundName != string.Empty)
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

		public void HandleDamage(int damage)
		{
			GD.Print($"Ow {damage}");
		}
	}
}