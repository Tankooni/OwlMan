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
            player.MovementInfo.VelY = 1200f;
			player.InputController.JumpSuccess();
		}

        public override void OnExit()
        {

			player.MovementInfo.ResetBoxes();
		}

        public override PlayerState Update()
        {
			player.MovementInfo.BottomBox = true;
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			player.MovementInfo.VelY += player.Gravity;
			if (signedHorizontal != 0)
				player.Image.FlipH = signedHorizontal < 0;
			player.MovementInfo.VelX = player.RunSpeed * Math.Sign(signedHorizontal);

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

			//TODO: Check for enemy collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null)
			// {
			// 	player.MovementInfo.VelY = 0;
			// 	enemy.World.Remove(enemy);
			// 	player.Energy++;
			//     last_bounce = delta;
			//     return new PSJump(player, 1.1f);
			// }

			return null;
        }
    }
}
