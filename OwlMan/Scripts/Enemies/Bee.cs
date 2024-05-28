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

		private bool isShooting = false;

		public override void _Ready()
		{
			base._Ready();

			Sprite2D.AnimationFinished += AnimatedSprite_AnimationFinished;
			Sprite2D.Play("idle");

			AddToGroup(HitGroups.Enemy);

			AddChild(shootAI = new ShootAt(Shoot, ChangeDirection, 60));
		}
		protected override void OnDamage(int damage, Damageable damageable)
		{
			Velocity = new Vector2(100, 100);
			MoveAndSlide();
			Velocity = Vector2.Zero;
		}

		private void AnimatedSprite_AnimationFinished()
		{
			if(isShooting)
			{
				Sprite2D.Play("idle");
				isShooting = false;
			}
		}

		public void Shoot()
		{
			isShooting = true;
			if (AttackSoundName != string.Empty)
				Overlord.OwlOverlord.PlaySound(AttackSoundName, this.GlobalPosition);
			Sprite2D.Play("attack");
		}

		public void ChangeDirection(Vector2 direction)
		{
			if (direction.X < 0)
				Sprite2D.FlipH = false;
			else if (direction.X > 0)
				Sprite2D.FlipH = true;
		}
	}
}