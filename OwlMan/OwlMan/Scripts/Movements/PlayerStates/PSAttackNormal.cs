using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Atmo2.Movements.PlayerStates
{
	class PSAttackNormal : PlayerState
	{
		private float speedModifier;

		public PSAttackNormal(Player player, float initialSpeedModifier)
			: base(player)
		{
			this.player = player;
			speedModifier = initialSpeedModifier;
		}

		public override void OnEnter()
		{
			player.Animation = "attackNormal";
		}

		public override void OnExit()
		{
			player.MovementInfo.VelX = 0;
			player.MovementInfo.ResetBoxes();
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			//Perform caluclations and modify player variables with results
			if (speedModifier != 0 && signedHorizontal != Math.Sign(speedModifier))
				speedModifier = 0;

			if (signedHorizontal != 0)
				player.Image.SetFlipH(signedHorizontal < 0);

			if (player.Image.FlipH)
			{
				player.MovementInfo.LeftBox = true;
			}
			else
			{
				player.MovementInfo.RightBox = true;
			}

			player.MovementInfo.VelX = player.RunSpeed * signedHorizontal + speedModifier;
			if (!player.IsOnFloor())
				player.MovementInfo.VelY += player.Gravity;

			//Handle any collision resitution & modify variables further if needed
			//player.image.SetSpeedScale(Math.Max(Math.Abs(h), .3f));
			//TODO: enemy collision
			// Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
			// if (enemy != null && !this.player.IsInvincable)
			// {
			//     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
			// }

			//Modify any timer variables & animations that will be based on movement
			if (speedModifier != 0)
			{
				speedModifier = Mathf.Clamp(speedModifier - player.HorizontalDrag * signedHorizontal, signedHorizontal < 0 ? speedModifier : 0, signedHorizontal < 0 ? 0 : speedModifier);
			}

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
