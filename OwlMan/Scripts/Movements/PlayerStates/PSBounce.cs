using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSBounce : PlayerState
	{
		private int duration;
		private float strX;
		private float strY;

		public PSBounce(Player player, float strX = 240, float strY = -120, int duration = 6)
			: base(player)
		{
			this.player = player;
			this.strX = strX;
			this.strY = strY;
			this.duration = duration;
		}
		public override void OnEnter()
		{
			this.player.Animation = "fall";

			//this.player.MovementInfo.VelY = strY;
		}

		public override void OnExit(PlayerState newState)
		{
			//player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			//Perform caluclations and modify player variables with results
			if (signedHorizontal != 0)
				player.FacingDirection = signedHorizontal;
			player.MovementInfo.Velocity.X = player.RunSpeed * signedHorizontal;

			//Handle any collision resitution & modify variables further if needed


			//Modify any timer variables & animations that will be based on movement
			--duration;

			//Handle Player input for state changers
			if (!player.IsOnFloor())
			{
				// && delta - PSDiveKick.last_bounce > 300)
				if (player.InputController.DashHeld() && player.InputController.DownHeld())
					return new PSDiveKick(player);

				if (player.Abilities.DoubleJump &&
					player.InputController.JumpPressedBuffered() &&
					player.Energy >= 1)
				{
					player.Energy -= 1;
					return new PSJump(player);
				}

				if (signedHorizontal != 0 &&
					player.Abilities.AirDash &&
					player.InputController.DashPressed() &&
					player.Energy >= 1)
				{
					player.Energy -= 1;
					return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection);
				}
			}
			else
			{
				if(player.InputController.JumpPressedBuffered())
				{
					return new PSJump(player);
				}
				if (player.Abilities.GroundDash &&
					player.InputController.DashPressed())
				{
					return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection);
				}
			}

			if (duration < 0)
			{
				if (player.IsOnFloor())
					if (player.InputController.LeftHeld() || player.InputController.RightHeld())
						return new PSRun(player);
					else
						return new PSIdle(player);
				else
					return new PSFall(player);
			}
			player.MovementInfo.Velocity.Y += player.Gravity;
			return null;
		}
	}
}
