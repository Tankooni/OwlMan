using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using System.Linq;

namespace Atmo2.Enemy.AI
{
	public partial class ShootAt : Node2D
	{
		public NodePath TargetPath { get; set; }
		
		Node2D target;

		//[Export]
		public List<string> TargetHitgroups { get; set; }

		private int attackFrameFrequency = 60;
		private int framesUntilAttack = 0;
		private int speed;
		Action shootCallback;
		Action<Vector2> directionCallback;

		public ShootAt(Action shootCallback, Action<Vector2> directionCallback, int attackFrameFrequency)
		{
			this.attackFrameFrequency = attackFrameFrequency;

			this.shootCallback = shootCallback;
			this.directionCallback = directionCallback;
		}

		public override void _Ready()
		{
			// Default to target's groups if none are given
			if (this.TargetHitgroups == null)
			{
				this.TargetHitgroups = new List<string>();
			}

			target = GetNode<Node2D>("../" + TargetPath.ToString());
		}

		public override void _PhysicsProcess(double delta)
		{
			if(target == null)
			{
				return;
			}

			var distance = GlobalPosition.DistanceTo(target.GlobalPosition);
			if (distance < 300)
			{
				var direction = GlobalPosition.DirectionTo(target.GlobalPosition);
				directionCallback(direction);

				if (framesUntilAttack == 0)
				{
					framesUntilAttack = (int)(GD.Randi() % 60) + attackFrameFrequency;
					Shoot(direction);
				}
				else
					framesUntilAttack = Math.Max(framesUntilAttack - 1, 0);
			}
		}

		void Shoot(Vector2 direction)
		{

			Projectile b = (Projectile)Overlord.Bullet.Instantiate();

			b.direction = direction;
			b.TargetHitgroups = TargetHitgroups;
			b.speed = 5;
			b.Position = GlobalPosition;

			GetNode("/root/GameScene").AddChild(b);

			if (shootCallback != null)
				shootCallback();
		}
	}
}