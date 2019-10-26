using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
    class PSOuch : PlayerState
    {
        private float duration;
        private int damage_taken;
		private float gravity;

        public PSOuch(Player player, int damage, float gravity)
			: base(player)
        {
            this.player = player;
            this.damage_taken = damage;
            this.duration = .4f;
			this.gravity = gravity;
        }
        public override void OnEnter()
        {
            player.image.Play("fall");
            player.Spice -= damage_taken;
            if (player.Spice == 0) return;

            player.MovementInfo.VelY = -240;
            if (player.image.FlipH)
                player.MovementInfo.VelX += 480;
            else
                player.MovementInfo.VelX -= 480;

            player.IsInvincable = true;
            // this.player.Tweener.Tween(this.player, new { Alpha = 1 }, .9f)
            //     .From(new { Alpha = 0})
            //     .Ease((t) => Math.Abs(player.Alpha - 1))
            //     .OnComplete(() =>
            //     {
            //         this.player.Alpha = 1;
            //         this.player.IsInvincable = false;
            //     });
        }

        public override void OnExit()
        {
            
        }

        public override PlayerState Update(float delta)
        {
            this.duration -= delta;
            if(this.duration < 0)
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
