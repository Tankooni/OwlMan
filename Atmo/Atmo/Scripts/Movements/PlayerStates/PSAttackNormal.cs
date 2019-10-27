using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Atmo2.Movements.PlayerStates
{
	class PSAttackNormal : PlayerState
	{
		public PSAttackNormal(Player player)
			: base(player)
		{
			this.player = player;
		}

		public override void OnEnter()
		{
			player.image.Play("attackNormal");
		}

		public override void OnExit()
		{
			player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update()
		{
			player.MovementInfo.VelY += player.Gravity;
			
			if(!player.MovementInfo.OnGround)
				player.MovementInfo.VelX = player.RunSpeed * Math.Sign(player.InputController.LeftStickHorizontal());

			//TODO: Enemy colision
			// Enemy enemy = player.World.CollideRect(KQ.CollisionTypeEnemy, player.X - player.HalfWidth + player.Width * (player.image.FlippedX ? -1 : 1), player.Y - player.Height, player.Width, player.Height) as Enemy;
			// if (enemy != null)
			// {
			// 	enemy.World.Remove(enemy);
			// }

			//Used to have an animcation callback here, but should no longer be needed

			return null;
		}

		public override PlayerState OnAnimationComplete()
		{
			if (player.MovementInfo.OnGround)
				if (player.InputController.LeftHeld() || player.InputController.RightHeld())
					return new PSRun(player);
				else
					return new PSIdle(player);
			else
				return new PSFall(player);
		}
	}
}
