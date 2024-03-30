using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
    class PSJump : PlayerState
    {
		private float multiplier;
		private float speedModifier;

		public PSJump(Player player, float initialSpeedModifier = 0, float multiplier = 1)
			: base(player)
		{
            this.player = player;
			this.multiplier = multiplier;
			speedModifier = initialSpeedModifier;
		}
        public override void OnEnter()
        {
            player.Animation = "jump";
			player.MovementInfo.Velocity.Y = -player.JumpStrenth * multiplier;
			player.InputController.JumpSuccess();
		}

        public override void OnExit(PlayerState newState)
        {
		}

        public override PlayerState Update()
        {
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			// MOVEMENT --------------------------------------------------------------------------
			player.MovementInfo.Velocity.X = player.RunSpeed * signedHorizontal + speedModifier;
			
			if (speedModifier != 0)
			{
				var modSign = Math.Sign(speedModifier);

				speedModifier = speedModifier - player.HorizontalAirDrag * modSign;

				if(modSign != Math.Sign(speedModifier))
					speedModifier = 0;
			}
			// ---------------------------------------------------------------------------------

			if (signedHorizontal != 0)
				player.FacingDirection = signedHorizontal;
			else if ( speedModifier != 0)
				player.FacingDirection = Math.Sign(speedModifier);

			//Handle Player input for state changers
			if (player.InputController.DashHeld() && player.InputController.DownHeld())
			{
				return new PSDiveKick(player);
			}

			if (player.Abilities.DoubleJump &&
				player.InputController.JumpPressedBuffered() &&
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSJump(player, speedModifier);
			}

			if (player.Abilities.AirDash &&
				player.InputController.DashPressed() &&
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection);
			}

			return new PSFall(player, speedModifier);
        }
    }
}
