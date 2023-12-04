using Godot;
using System;
using System.Runtime.InteropServices;

public partial class Flagpole : Area2D
{

	// Signal to notify when the player touches the flag
    [Signal]
    public delegate void PlayerTouchedEventHandler();

    private CollisionShape2D _collisionShape2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		this.AreaEntered += OnArea2DAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnArea2DAreaEntered(Area2D otherArea)
	{
		// Assuming you have a reference to the AnimatedSprite2D node
		AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Change the animation to a new animation named "new_animation"
		animatedSprite.Play("owl");
	}

}

