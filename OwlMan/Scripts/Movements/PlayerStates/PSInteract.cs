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
		public PSInteract(Player player)
			: base(player)
		{
			this.player = player;
		}

		public override void OnEnter()
		{
			player.MovementInfo.VelY = 0;
			// player.MovementInfo.VelX = 0;
			player.Animation = "idle";
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


			// TODO:  Check for enemy collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null && !this.player.IsInvincable)
			// {
			//     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
			// }

			if (!player.MovementInfo.OnGround)
			{
				return new PSFall(player);
			}

			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, 0);
			}
			if (player.InputController.DownPressed())
			{
				return new PSCharge(player);
			}
			if (signedHorizontal != 0)
			{
				return new PSRun(player);
			}
			if(player.InputController.JumpPressed())
			{
				return new PSJump(player);
			}

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
