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

		// this.AreaEntered += OnArea2DAreaEntered;
		this.BodyEntered += OnBodyEntered;
	}

    private void OnBodyEntered(Node2D body)
    {
		if(body.IsInGroup(HitGroups.Player))
		{
			AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
			// Change the animation to a new animation named "new_animation"
			animatedSprite.Play("owl");
		}
    }

}

