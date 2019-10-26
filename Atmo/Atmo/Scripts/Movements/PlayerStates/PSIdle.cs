using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
// using Atmo2.Entities;

namespace Atmo2.Movements.PlayerStates
{
    class PSIdle : PlayerState
    {
        public PSIdle(Player player)
			: base(player)
		{
            this.player = player;
        }

        public override void OnEnter()
        {
			player.MovementInfo.VelY = 0;
			player.image.Play("idle");
        }

        public override void OnExit()
        {
        }

        public override PlayerState Update(float delta)
        {
            player.RefillEnergy(delta);

			

            // See if there's ground below us
            if(!player.MovementInfo.OnGround)
			{
                return new PSFall(player, KQ.STANDARD_GRAVITY);
            }
			// TODO:  Check for enemy collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null && !this.player.IsInvincable)
			// {
			//     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
			// }
			if (player.Abilities.GroundDash &&
				Controller.DashPressed())
			{
				if (Controller.LeftStickHorizontal() != 0 || Controller.LeftStickVertical() != 0)
					return new PSDash(player);
			}

			if (Controller.AttackPressed())
			{
				return new PSAttackNormal(player, KQ.STANDARD_GRAVITY);
			}
            if(Controller.DownPressed())
            {
               return new PSCharge(player);
            }
            if(Controller.LeftHeld() || Controller.RightHeld())
            {
                return new PSRun(player);
            }
            if(Controller.JumpPressed())
            {
                return new PSJump(player);
            }

            return null;
        }
    }
}
