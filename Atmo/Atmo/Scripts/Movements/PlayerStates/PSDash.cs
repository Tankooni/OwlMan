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
        private float speed;
        private float duration;
		private Vector2 direction;

        public PSDash(Player player,/* float direction,*/ float speed = -1, float duration = .3f)
			: base(player)
		{
            this.player = player;
            this.speed = speed < 0 ? 3.5f * player.RunSpeed : speed;
			//this.direction = direction;
            this.duration = duration;

			player.MovementInfo.VelX = 0;
			player.MovementInfo.VelY = 0;
		}
        public override void OnEnter()
        {
            player.image.Play("dash");
			direction = new Vector2(Controller.LeftStickHorizontal(), Controller.LeftStickVertical()).Normalized();
		}

        public override void OnExit()
        {
        }

        public override PlayerState Update(float delta)
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
			player.MovementInfo.VelX = this.speed * direction.x;
			player.MovementInfo.VelY = this.speed * direction.y;

			/*
			(-0.921875, -1)(-0.6778028, -0.7352437)
			(-1, 0)(-1, 0)
			(1, 0)(1, 0)
			(1, -1)(0.7071068, -0.7071068)
			*/

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
			}

			if (player.MovementInfo.OnGround && Controller.JumpPressed())
				return new PSJump(player);
			
			duration -= delta;
            if(duration < 0)
            {
                // We're done here
                if (player.MovementInfo.OnGround)
                    if (Controller.LeftHeld() || Controller.RightHeld())
                        return new PSRun(player);
                    else
                        return new PSIdle(player);
                else
                    return new PSFall(player, KQ.STANDARD_GRAVITY);
            }
            return null;
        }
    }
}
