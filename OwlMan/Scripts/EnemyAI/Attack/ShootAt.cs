using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using System.Linq;

namespace Atmo2.Enemy.AI
{
	public partial class ShootAt : Node2D
	{
		// public NodePath TargetPath { get; set; }
		
		private Node2D target;
		public Node2D Target
		{
			get
			{	
				if(target == null)
				{
					if( Enemy.PlayerPath == null)
						return null;
					target = GetTree().CurrentScene.GetNodeOrNull<Targetable>( $"{Enemy.PlayerPath.ToString()}/{nameof(Targetable)}" );
				}
				return target ??= GetTree().CurrentScene.GetNodeOrNull<Node2D>( Enemy.PlayerPath );
			}
		}

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

		}

		public override void _PhysicsProcess(double delta)
		{
			if(Target == null)
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

			b.Name = "Projectile";
			b.direction = direction;
			b.speed = 5;
			b.Position = GlobalPosition;

			GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, b);

			if (shootCallback != null)
				shootCallback();
		}
	}
}