using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
// using Atmo2.Entities;

namespace Atmo2.Movements.PlayerStates
{
	class PSInteract : PlayerState
	{
		private Control DialogueInstance = null;
		private Node CurrentScene = null;
		public PSInteract(Player player)
			: base(player)
		{
			this.player = player;
		}

		public override void OnEnter()
		{
			//// player.MovementInfo.VelY = 0;
			//// // player.MovementInfo.VelX = 0;
			//// player.Animation = "idle";
			//CurrentScene = GetTree().CurrentScene as Node;
			//if (DialogueInstance == null && CurrentScene != null)
			//{
				//DialogueInstance = (Control)GD.Load<PackedScene>("res://path/to/DialogueScene.tscn").Instance();
//
				//// Add the dialogue instance to the current scene
				//CurrentScene.AddChild(DialogueInstance);
//
				//DialogueInstance.Position = new Vector2(100, 100);
				//DialogueInstance.Size = new Vector2(400, 200);
			//}
		}

		public override void OnExit(PlayerState newState)
		{
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			//Perform caluclations and modify player variables with results
			player.RefillEnergy();


			


			return null;
		}


		// public static bool CheckInteract(Player player)
		// {
		// 	bool result = player.OverlapsArea(player.GlobalPosition, HitGroups.GetGroups(HitGroups.Interact));
		// 	GD.print("CheckInteract Result: ", result);
		// 	return result;
		// 	// foreach (string hitgroup in HitGroups.GetGroups())
		// 	// {
		// 	// 	if (player.OverlapsArea(player.GlobalPosition, HitGroups.GetGroups(hitgroup)))
		// 	// }	
		// 	// return false;


		// 	// // Check if player overlaps with an interactable object
		// 	// if ()
		// 	// {
		// 	// 	return false;
		// 	// }
			
		// }
	}
}
