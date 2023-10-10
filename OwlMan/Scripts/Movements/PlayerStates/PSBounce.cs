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

		public override void OnExit()
		{
			//player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			//Perform caluclations and modify player variables with results
			if (signedHorizontal != 0)
				player.Image.FlipH = signedHorizontal < 0;
			player.MovementInfo.VelX = player.RunSpeed * signedHorizontal;

			//Handle any collision resitution & modify variables further if needed


			//Modify any timer variables & animations that will be based on movement
			--duration;

			//Handle Player input for state changers
			if (!player.MovementInfo.OnGround)
			{
				// && delta - PSDiveKick.last_bounce > 300)
				if (player.InputController.JumpPressed() && player.InputController.DownHeld())
					return new PSDiveKick(player);

				if (player.Abilities.DoubleJump &&
					player.InputController.JumpPressed() &&
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
					if (player.InputController.LeftStickHorizontal() != 0 || player.InputController.LeftStickVertical() != 0)
					{
						player.Energy -= 1;
						return new PSDash(player, signedHorizontal);
					}
				}
			}
			else
			{
				if(player.InputController.JumpPressed())
					return new PSJump(player);
				if (signedHorizontal != 0 &&
					player.Abilities.GroundDash &&
					player.InputController.DashPressed())
				{
					return new PSDash(player, signedHorizontal);
				}
			}

			if (duration < 0)
			{
				if (player.MovementInfo.OnGround)
					if (player.InputController.LeftHeld() || player.InputController.RightHeld())
						return new PSRun(player);
					else
						return new PSIdle(player);
				else
					return new PSFall(player);
			}
			player.MovementInfo.VelY += player.Gravity;
			return null;
		}
	}
}
