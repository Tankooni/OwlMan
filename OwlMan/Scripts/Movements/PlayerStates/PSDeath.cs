using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
    class PSDeath : PlayerState
    {
        public PSDeath(Player player)
			: base(player)
		{
            this.player = player;
        }
        public override void OnEnter()
        {
            // player.Collidable = false;
            player.Animation = "fall";
            // player.Tweener.Tween(player, new { Alpha = 1 }, 1)
            //     .From(new { Alpha = 0 })
            //     .Ease((t) => Engine.Random.Float(0, 1))
            //     .OnComplete(() => player.Alpha = 1);
            // player.Tweener.Tween(player, new { X = player.resetPointX, Y = player.resetPointY}, 1)
            //     .Ease((t) => t)
            //     .OnComplete(() => this.animation_finished = true);
        }

        public override void OnExit()
        {
            // player.Collidable = true;
        }

        public override PlayerState Update()
        {
            // if(animation_finished)
            // {
                player.ResetPlayerPosition();
                player.Spice = 100;
                return new PSJump(player);
            // }
        }
    }
}
