using Godot;
using Atmo2.Enemy.AI;
using System.Collections.Generic;
using System.Numerics;

namespace Atmo2.Enemy
{
	public abstract partial class Enemy : CharacterBody2D
	{
		// So this is maybe not the best design but it is by far the most convenient and not as hacky solution. This should eventually be replaced with some sort of AI player detection system.
		public static NodePath PlayerPath;

		[Export]
		public AnimatedSprite2D Sprite2D_Old;
		[Export]
		public Sprite2D Sprite;
		[Export]
		public AnimationPlayer AnimPlayer;
		[Export]
		public Damageable Damageable;

		public override void _EnterTree()
		{
			base._EnterTree();
		}

		public override void _ExitTree()
		{
			base._ExitTree();
		}

		public override void _Ready()
		{
			Damageable.OnDeathCallback += OnDeath;
			Damageable.OnDamageCallback += OnDamage; 
		}

		// public override void _PhysicsProcess(double delta)
		// {
			
		// }
		protected virtual void OnDamage(int damage, Damageable damageable)
		{

		}
		protected virtual void OnDeath()
		{
			QueueFree();
		}

	}
}