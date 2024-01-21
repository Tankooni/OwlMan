using Godot;
using System;

public partial class InteractComponent : Area2D
{
	[Export]
	public string IDLabel;
	// Called when the node enters the scene tree for the first time.
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
				GD.Print("Interact Component exited");
			}
	}
}



