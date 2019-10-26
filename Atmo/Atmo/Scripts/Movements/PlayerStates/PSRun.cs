using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSRun : PlayerState
	{
		public PSRun(Player player)
			:base(player)
		{
			this.player = player;
		}
		public override void OnEnter()
		{
			player.MovementInfo.VelY = 0;
			player.image.Play("walk");
		}

		public override void OnExit()
		{
			player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update(float delta)
		{
			player.RefillEnergy(delta);

			if (Controller.AttackPressed())
				return new PSAttackNormal(player, KQ.STANDARD_GRAVITY);

			var h = Controller.LeftStickHorizontal();
			if (h == 0)
				return new PSIdle(player);
			player.image.SetFlipH(h < 0);
			player.MovementInfo.VelX = player.RunSpeed * Math.Sign(h);
			//player.image.SetSpeedScale(Math.Max(Math.Abs(h), .3f));
			//TODO: enemy collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null && !this.player.IsInvincable)
			// {
			//     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
			// }

			if (Controller.JumpPressed())
			{
				return new PSJump(player);
			}
			if(player.Abilities.GroundDash && 
				Controller.DashPressed())
			{
				if (Controller.LeftStickHorizontal() != 0 || Controller.LeftStickVertical() != 0)
					return new PSDash(player);
			}
			if (!player.MovementInfo.OnGround)
			{
				return new PSFall(player, KQ.STANDARD_GRAVITY);
			}

			return null;
		}
	}
}
