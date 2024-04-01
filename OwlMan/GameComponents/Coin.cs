using Godot;
using System;

public partial class Coin : Area2D
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void PlayerTouchedEventHandler();
	private CollisionShape2D CoinCollision;
	public override void _Ready()
	{
		CoinCollision = GetNode<CollisionShape2D>("CoinCollision");
		this.BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body.IsInGroup(HitGroups.Player))
		{
			Overlord.HudLayer.AddCoinCount();
			QueueFree();
		}
	}
}
