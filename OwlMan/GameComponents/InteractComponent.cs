using Godot;
using System;
using System.Collections.Generic;

public partial class InteractComponent : Area2D
{
	[ExportSubgroup("Interact Properties")]
	[Export]
	public string IDLabel;

	public enum EnumItems
{

    Umbrella = 1,
    Ball = 2,
    Hat = -1,
}

	[Export]
	public EnumItems SpecialItems { get; set; }

	// Called when the node enters the scene tree for the first time.

	[Export]
	public string[] Options { get; set; } = { "Option 1", "Option 2", "Option 3" };


	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body.IsInGroup(HitGroups.Player))
			{
				GD.Print("Interact Component entered");
				
				//Player player = GetNode<Player>("Player");
				Player player = body as Player;

				GD.Print(player);
				player.Interactable = this;
				player.IDLabel = IDLabel;
				GD.Print(this);


				// if (player != null)
				// {
				// 	GD.Print("Player found");
				// 	player.Interactable = this;
				// 	GD.Print(this);


				// }

			}
	}



	private void OnBodyExited(Node2D body)
	{
		if(body.IsInGroup(HitGroups.Player))
			{
				Player player = body as Player;
				GD.Print("Interact Component exited");
				player.Interactable = null;
				
			}
	}
}



