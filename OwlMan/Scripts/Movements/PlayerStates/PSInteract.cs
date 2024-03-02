using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
		public bool isReadyToClose = false;
		public PSInteract(Player player)
			: base(player)
		{
			this.player = player;
		}

		public override void OnEnter()
		{
			GD.Print("State Interact Entered");
			Overlord.DialogueScripts.SetVisible(true);
			Overlord.DialogueScripts.ParseJSON(player.IDLabel);
			
			// // player.MovementInfo.VelY = 0;
			// // // player.MovementInfo.VelX = 0;
			// // player.Animation = "idle";

			// // Load the dialogue scene
			// PackedScene DialogueScene = (PackedScene)GD.Load("res://Dialogue/Dialogue.tscn");

			// // Instance the dialogue scene
			// Node dialogueNode = DialogueScene.Instance() as Node;

			// if (dialogueNode != null)
			// {
			// 	// Add the dialogue instance to the current scene
			// 	GetTree().CurrentScene.AddChild(dialogueNode);

			// 	// Set position and size (optional)
			// 	dialogueNode.Position = new Vector2(100, 100);
			// 	dialogueNode.RectSize = new Vector2(400, 200);
			// 	}
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

			if (player.InputController.InteractPressed() && Overlord.DialogueScripts.IsReadyToClose)
			{
				Overlord.DialogueScripts.SetVisible(false);
				return new PSIdle(player);
			}
			if ((player.InputController.InteractPressed() || player.InputController.JumpPressed()) && !Overlord.DialogueScripts.IsReadyToClose)
			{
				Overlord.DialogueScripts.ContinueDialogueBox();
			}
			

			return null;

		}

		private void _OnDialogueReadyToClose()
        {
            GD.Print("Dialogue is ready to close!");
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
