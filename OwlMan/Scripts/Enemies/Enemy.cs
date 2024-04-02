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

		public void LootDropCalculation()
		{
			Godot.Vector2 position = GlobalPosition;
			GD.Print("Enemy destroyed at position: ", position);

			QueueFree();

			// // Load the Coin scene
			// PackedScene coinScene = (PackedScene)GD.Load("res://GameComponents/Coin.tscn");

			// // Check if the Coin scene loaded successfully
			// if (coinScene != null)
			// {
			// 	// Instantiate the Coin scene
			// 	Node coinNode = coinScene.Instance();

			// 	// Set the position of the Coin node
			// 	coinNode.GlobalPosition = position;

			// 	// Add the Coin node to the scene tree
			// 	GetTree().Root.AddChild(coinNode);

			// 	// Print a message to verify
			// 	GD.Print("Coin generated at position: ", position);
			// }
			// else
			// {
			// 	GD.PrintErr("Failed to load Coin scene.");
			// }
		}
	}
}