using Godot;
using Atmo2.Enemy.AI;
using System.Collections.Generic;

namespace Atmo2.Enemy
{
	public abstract partial class Enemy : CharacterBody2D
	{
		// So this is maybe not the best design but it is by far the most convenient and not as hacky solution. This should eventually be replaced with some sort of AI player detection system.
		public static NodePath PlayerPath;

		[Export]
		public AnimatedSprite2D Sprite2D;
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

		// public override void _Ready()
		// {
		// }

		// public override void _PhysicsProcess(double delta)
		// {
			
		// }
	}
}