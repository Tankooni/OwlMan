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

		public PSJump(Player player, float multiplier = 1)
			: base(player)
		{
            this.player = player;
			this.multiplier = multiplier;

		}
        public override void OnEnter()
        {
            player.image.Play("jump");
			player.MovementInfo.VelY = 0;
		}

        public override void OnExit()
        {
		}

        public override PlayerState Update(float delta)
        {
			//TODO: enemy collision
            // Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            // if (enemy != null && !this.player.IsInvincable)
            // {
            //     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
            // }

            player.MovementInfo.VelY = -player.JumpStrenth * multiplier;
            return new PSFall(player, KQ.STANDARD_GRAVITY);
        }
    }
}
