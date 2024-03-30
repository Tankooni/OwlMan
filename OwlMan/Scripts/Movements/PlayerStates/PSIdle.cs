using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
// using Atmo2.Entities;

namespace Atmo2.Movements.PlayerStates
{
	class PSIdle : PlayerState
	{
		public PSIdle(Player player)
			: base(player)
		{
			this.player = player;
		}

		public override void OnEnter()
		{
			player.MovementInfo.Velocity.Y = 0;
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

			if (!player.IsOnFloor())
			{
				return new PSFall(player, coyoteTime: true);
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
			if(player.InputController.JumpPressedBuffered())
			{
				return new PSJump(player);
			}

			if (player.InputController.InteractPressed() && player.HasInteract())
			{
				GD.Print("IDLE TO INTERACT STATE CHANGED");
				return new PSInteract(player);
			}

			if(player.Abilities.GroundDash && 
				player.InputController.DashPressed())
			{
				return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection, coyoteTime: true);
			}

			return null;
		}
	}
}
