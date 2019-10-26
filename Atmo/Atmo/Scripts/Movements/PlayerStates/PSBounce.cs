using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSBounce : PlayerState
	{
		private float duration;
		private float strX;
		private float strY;
		private float gravity;

		public PSBounce(Player player, float gravity, float strX = 240, float strY = -120, float duration = .1f)
			: base(player)
		{
			this.player = player;
			this.gravity = gravity;
			this.strX = strX;
			this.strY = strY;
			this.duration = duration;
		}
		public override void OnEnter()
		{
			this.player.image.Play("fall");

			//this.player.MovementInfo.VelY = strY;
		}

		public override void OnExit()
		{
			//player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update(float delta)
		{
			duration -= delta;

			if (!player.MovementInfo.OnGround)
			{
				// && delta - PSDiveKick.last_bounce > 300)
				if (Controller.JumpPressed() && Controller.DownHeld())
					return new PSDiveKick(player, KQ.STANDARD_GRAVITY);

				if (player.Abilities.DoubleJump &&
					Controller.JumpPressed() &&
					player.Energy >= 1)
				{
					player.Energy -= 1;
					return new PSJump(player);
				}

				if (player.Abilities.AirDash &&
					Controller.DashPressed() &&
					player.Energy >= 1)
				{
					if (Controller.LeftStickHorizontal() != 0 || Controller.LeftStickVertical() != 0)
					{
						player.Energy -= 1;
						return new PSDash(player);
					}
				}
			}
			else
			{
				if(Controller.JumpPressed())
					return new PSJump(player);
				if (player.Abilities.GroundDash &&
					Controller.DashPressed())
					if (Controller.LeftStickHorizontal() != 0 || Controller.LeftStickVertical() != 0)
						return new PSDash(player);
			}

			player.MovementInfo.VelX = player.RunSpeed * Math.Sign(Controller.LeftStickHorizontal());

			if (duration < 0)
			{
				if (player.MovementInfo.OnGround)
					if (Controller.LeftHeld() || Controller.RightHeld())
						return new PSRun(player);
					else
						return new PSIdle(player);
				else
					return new PSFall(player, KQ.STANDARD_GRAVITY);
			}
			player.MovementInfo.VelY += gravity;
			return null;
		}
	}
}
