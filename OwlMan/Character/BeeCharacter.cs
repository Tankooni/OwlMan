using Godot;
using System;

public partial class BeeCharacter : CharacterBody2D
{
	public override void _Ready()
		{
			base._Ready();

			AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite.Play("idle");
		}
}
