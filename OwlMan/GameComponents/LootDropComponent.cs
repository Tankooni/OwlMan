using Atmo2;
using Godot;
using System;

[GlobalClass]
public partial class LootDropComponent : Node2D
{
	[ExportSubgroup("LootDrop Properties")]
	[Export]
	public Damageable Damageable;
	[Export]
	public int NumGold;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Damageable.OnDeathCallback += DropLoop;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DropLoop()
	{
		GD.Print("DROPPING LOOT");
		Vector2 position = GlobalPosition;
		GD.Print("Enemy destroyed at position: ", position);

		for (int i = 0; i < NumGold; i++)
    	{
			Coin Coin = (Coin)Overlord.Coin.Instantiate();
			Coin.Name = "Coin";
			Coin.Position = position + new Vector2(i * 20, 0);

			GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, Coin);
		}
	}
}
