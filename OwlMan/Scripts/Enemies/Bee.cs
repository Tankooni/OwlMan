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
		public NodePath Target
		{
			get { return this.shootAI.Target; }
			set { this.shootAI.Target = value; }
		}

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

			AddChild(shootAI = new ShootAt(Shoot, ChangeDirection, 10)
			{
				TargetHitgroups = new List<string> { HitGroups.Player, HitGroups.Wall }
			});
		}

		private void AnimatedSprite_AnimationFinished()
		{
			GD.Print("Bee anim finished");
			if(isShooting)
			{
				animatedSprite.Play("idle");
				isShooting = false;
			}
		}

		public void Shoot()
		{
			GD.Print("Shoot");
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
	}
}