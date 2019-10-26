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
		private float gravity;

		public PSDiveKick(Player player, float gravity)
			: base(player)
		{
            this.player = player;
			this.gravity = gravity;
		}
        public override void OnEnter()
        {
            player.image.Play("diveKick");
            player.MovementInfo.VelY = 1200f;
        }

        public override void OnExit()
        {

        }

        public override PlayerState Update(float delta)
        {
			player.MovementInfo.VelY += gravity;

			if (player.MovementInfo.OnGround)
            {
				if (Controller.LeftHeld() || Controller.RightHeld())
					return new PSRun(player);
				else
					return new PSIdle(player);
			}

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

			var h = Controller.LeftStickHorizontal();
			if (h != 0)
				player.image.SetFlipH(h < 0);
			player.MovementInfo.VelX = player.RunSpeed * Math.Sign(h);

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
