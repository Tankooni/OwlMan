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
		private float speedModifier;
		

		public PSRun(Player player, float initialSpeedModifier = 0)
			:base(player)
		{
			this.player = player;
			speedModifier = initialSpeedModifier;
		}
		public override void OnEnter()
		{
			player.MovementInfo.VelY = 0;
			player.Animation = "walk";
		}

		public override void OnExit(PlayerState newState)
		{
			player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			//Perform caluclations and modify player variables with results
			if (speedModifier != 0 && signedHorizontal != Math.Sign(speedModifier))
				speedModifier = 0;

			player.RefillEnergy();

			if (signedHorizontal == 0)
				return new PSIdle(player);
			player.Image.FlipH = signedHorizontal < 0;

			player.MovementInfo.VelX = player.RunSpeed * signedHorizontal + speedModifier;

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

			//Handle Player input for state changers
			if (player.InputController.AttackPressed())
				return new PSAttackNormal(player, speedModifier);

			if (player.InputController.JumpPressed())
			{
				return new PSJump(player);
			}
			if(player.Abilities.GroundDash && 
				player.InputController.DashPressed())
			{
				if (signedHorizontal != 0)
					return new PSDash(player, signedHorizontal);
			}
			if (!player.MovementInfo.OnGround)
			{
				return new PSFall(player, coyoteTime: true);
			}
			if (!player.InputController.InteractPressed() && player.HasInteract())
			{
				return new PSInteract(player);
			}

			return null;
		}
	}
}
