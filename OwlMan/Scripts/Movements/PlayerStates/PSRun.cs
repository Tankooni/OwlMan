using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSRun : PlayerState
	{
		private float speedModifier;
		

		public PSRun(Player player, float initialSpeedModifier = 0)
			:base(player)
		{
			this.player = player;
			speedModifier = initialSpeedModifier;
		}
		public override void OnEnter()
		{
			player.MovementInfo.Velocity.Y = 0;
			player.Animation = "walk";
		}

		public override void OnExit(PlayerState newState)
		{
			player.MovementInfo.Velocity.X = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			player.RefillEnergy();

			// MOVEMENT --------------------------------------------------------------------------
			player.MovementInfo.Velocity.X = player.RunSpeed * signedHorizontal + speedModifier;
			
			if (speedModifier != 0)
			{
				var modSign = Math.Sign(speedModifier);

				speedModifier = speedModifier - player.HorizontalGroundDrag * modSign;

				if(modSign != Math.Sign(speedModifier))
					speedModifier = 0;
			}
			// ---------------------------------------------------------------------------------

			if (signedHorizontal != 0)
				player.FacingDirection = signedHorizontal;
			else if ( speedModifier != 0)
				player.FacingDirection = Math.Sign(speedModifier);

			if (signedHorizontal == 0 && speedModifier == 0)
			{
				return new PSIdle(player);
			}

			//Handle Player input for state changers
			if (player.InputController.AttackPressed())
				return new PSAttackNormal(player, speedModifier);

			if (player.InputController.JumpPressedBuffered())
			{
				return new PSJump(player, speedModifier);
			}
			if(player.Abilities.GroundDash && 
				player.InputController.DashPressed())
			{
				return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection, coyoteTime: true);
			}
			if (!player.IsOnFloor())
			{
				return new PSFall(player, coyoteTime: true);
			}
			if (player.InputController.InteractPressed() && player.HasInteract())
			{
				GD.Print("IDLE TO INTERACT STATE CHANGED");
				return new PSInteract(player);
			}

			return null;
		}
	}
}
