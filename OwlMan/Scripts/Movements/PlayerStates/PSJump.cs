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
			player.MovementInfo.VelY = -player.JumpStrenth * multiplier;
			player.InputController.JumpSuccess();
		}

        public override void OnExit(PlayerState newState)
        {
		}

        public override PlayerState Update()
        {
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());
			
			//Perform caluclations and modify player variables with results
			if (speedModifier != 0 && signedHorizontal != Math.Sign(speedModifier))
				speedModifier = 0;

			if (signedHorizontal != 0)
				player.Image.FlipH = signedHorizontal < 0;

			player.MovementInfo.VelX = player.RunSpeed * signedHorizontal + speedModifier;

			//Handle any collision resitution & modify variables further if needed
			//TODO: enemy collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null && !this.player.IsInvincable)
			// {
			//     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
			// }
			

			//Modify any timer variables & animations that will be based on movement
			if (speedModifier != 0)
			{
				speedModifier = Mathf.Clamp(
					speedModifier - player.HorizontalDrag * signedHorizontal,
					signedHorizontal < 0 ? speedModifier : 0,
					signedHorizontal < 0 ? 0 : speedModifier);
			}

			//Handle Player input for state changers
			if (player.InputController.JumpPressed() && player.InputController.DownHeld())
				return new PSDiveKick(player);

			if (player.Abilities.DoubleJump &&
				player.InputController.JumpPressed() &&
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSJump(player, speedModifier);
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

			return new PSFall(player, speedModifier);
        }
    }
}
