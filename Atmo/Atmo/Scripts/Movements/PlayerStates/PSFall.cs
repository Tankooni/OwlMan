using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSFall : PlayerState
	{
		private float gravity;
		private float speed;

		public PSFall(Player player, float gravity, float speed = -1)
			: base(player)
		{
			this.player     = player;
			this.gravity    = gravity;
			this.speed      = speed < 0 ? player.RunSpeed : speed;
		}
		public override void OnEnter()
		{
			AnimationCheckSet();
		}

		public override void OnExit()
		{
			player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update(float delta)
		{
			player.MovementInfo.VelY += gravity;
			if (player.MovementInfo.HeadBonk)
				player.MovementInfo.VelY = gravity;


			AnimationCheckSet();

			if (Controller.AttackPressed())
			{
				return new PSAttackNormal(player, KQ.STANDARD_GRAVITY);
			}
			// && delta - PSDiveKick.last_bounce > 300)
			if (Controller.JumpPressed() && Controller.DownHeld())
				return new PSDiveKick(player, KQ.STANDARD_GRAVITY);

			var h = Controller.LeftStickHorizontal();
			if (h != 0)
				player.image.SetFlipH(h < 0);
			player.MovementInfo.VelX = player.RunSpeed * Math.Sign(h);

			//TODO: Enemy Collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null && !this.player.IsInvincable)
			// {
			//     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
			// }

			if (player.Abilities.DoubleJump &&
				Controller.JumpPressed() && 
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSJump(player);
			}

			if(player.Abilities.AirDash && 
				Controller.DashPressed() && 
				player.Energy >= 1)
			{
				if (Controller.LeftStickHorizontal() != 0 || Controller.LeftStickVertical() != 0)
				{
					player.Energy -= 1;
					return new PSDash(player);
				}
			}

			if (player.MovementInfo.OnGround)
				if (player.MovementInfo.VelX == 0)
					return new PSIdle(player);
				else // Hit the ground runnin'
					return new PSRun(player);

			return null;
		}
		private void AnimationCheckSet()
		{
			if (player.MovementInfo.VelY > 0)
				player.image.Play("fall");
			else
				player.image.Play("jump");
		}
	}
}
