using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
    class PSDash : PlayerState
    {
        private readonly float speed;
        private int dashTicks;
		private float direction;

		private float SpeedModifier { get { return player.DashMultiplier * player.RunSpeed * direction; } }

        public PSDash(Player player, float direction, int dashTicks = 10) //dash should probably be 9-10
			: base(player)
		{
            this.player = player;
            this.speed = player.DashMultiplier * player.RunSpeed;
			this.direction = direction;
            this.dashTicks = dashTicks;

			player.MovementInfo.VelX = this.speed * direction;
			player.MovementInfo.VelY = 0;
		}
        public override void OnEnter()
        {
            player.Animation = "dash";
			//direction = new Vector2(player.InputController.LeftStickHorizontal(), 0/*Controller.LeftStickVertical()*/).Normalized();
		}

        public override void OnExit()
		{
			//player.MovementInfo.ResetBoxes();
		}

        public override PlayerState Update()
        {
			//TODO: Enemy colision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null /*&& !this.player.IsInvincable*/)
			// {
			// 	//player.MovementInfo.Move = Math.Sign(player.MovementInfo.Move) * 100;
			// 	enemy.World.Remove(enemy);
			// 	player.Energy++;
			// 	//return new PSFall(player);
			// 	return new PSBounce(player, KQ.STANDARD_GRAVITY/*, enemy.touchDamage*/);
			// }

			//TODO: change this to be directional based
			player.MovementInfo.VelX = this.speed * direction;
			//player.MovementInfo.VelY = this.speed * direction.y;

			//if (player._image.FlipH)
			//{
			//	player.MovementInfo.LeftBox = true;
			//}
			//else
			//{
			//	player.MovementInfo.RightBox = true;
			//}

			/*
			(-0.921875, -1)(-0.6778028, -0.7352437)
			(-1, 0)(-1, 0)
			(1, 0)(1, 0)
			(1, -1)(0.7071068, -0.7071068)
			*/

			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, SpeedModifier);
			}

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
					return new PSJump(player, SpeedModifier);
				}
			}

			if (player.MovementInfo.OnGround && player.InputController.JumpPressed())
			{
				return new PSJump(player, SpeedModifier);
			}

			--dashTicks;
            if(dashTicks < 0)
            {
				// We're done here
				if (player.MovementInfo.OnGround)
					if (player.InputController.LeftHeld() || player.InputController.RightHeld())
						return new PSRun(player, SpeedModifier);
					else
						return new PSIdle(player);
				else
				{
					return new PSFall(player, SpeedModifier);
				}
            }
            return null;
        }
    }
}
