using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Atmo2.Movements.PlayerStates
{
	class PSAttackNormal : PlayerState
	{
		private float gravity;
		public PSAttackNormal(Player player, float gravity)
			: base(player)
		{
			this.player = player;
			this.gravity = gravity;
		}

		public override void OnEnter()
		{
			player.image.Play("attackNormal");
		}

		public override void OnExit()
		{
			player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update(float delta)
		{
			player.MovementInfo.VelY += gravity;
			
			if(!player.MovementInfo.OnGround)
				player.MovementInfo.VelX = player.RunSpeed * Math.Sign(Controller.LeftStickHorizontal());

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
				if (Controller.LeftHeld() || Controller.RightHeld())
					return new PSRun(player);
				else
					return new PSIdle(player);
			else
				return new PSFall(player, KQ.STANDARD_GRAVITY);
		}
	}
}
