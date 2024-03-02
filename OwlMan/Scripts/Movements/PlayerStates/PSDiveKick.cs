using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSDiveKick : PlayerState
	{
		//TODO: Fix this
		public static float last_bounce;

		public PSDiveKick(Player player)
			: base(player)
		{
			this.player = player;
		}
		public override void OnEnter()
		{
			player.Animation = "diveKick";
			player.MovementInfo.Velocity.Y = player.Gravity * 20f;
			player.InputController.JumpSuccess();
		}

		public override void OnExit(PlayerState newState)
		{

			player.MovementInfo.ResetBoxes();
		}

		public override PlayerState Update()
		{
			player.MovementInfo.BottomTrace = true;
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			player.MovementInfo.Velocity.Y += player.Gravity * 2;

			if (signedHorizontal != 0)
				player.FacingDirection = signedHorizontal;
			player.MovementInfo.Velocity.X = player.RunSpeed * Math.Sign(signedHorizontal);

			if (player.MovementInfo.OnGround)
			{
				if (player.InputController.LeftHeld() || player.InputController.RightHeld())
					return new PSRun(player);
				else
					return new PSIdle(player);
			}

			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, 0);
			}

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

			return null;
		}
	}
}
