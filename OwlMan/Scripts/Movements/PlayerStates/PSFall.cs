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
		private float speed;
		private int coyoteTimeTicks;
		private float speedModifier;

		public PSFall(Player player, float initialSpeedModifier = 0, bool coyoteTime = false, float speed = -1)
			: base(player)
		{
			this.player = player;
			this.speed = speed < 0 ? player.RunSpeed : speed;
			this.coyoteTimeTicks = coyoteTime ? 10 : 0;
			speedModifier = initialSpeedModifier;
		}
		public override void OnEnter()
		{
			AnimationCheckSet();
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
			{
				speedModifier = 0;
			}

			player.MovementInfo.VelY += player.Gravity;
			if (player.MovementInfo.HeadBonk)
				player.MovementInfo.VelY = player.Gravity;
			
			if (signedHorizontal != 0)
				player.Image.FlipH = signedHorizontal < 0;
			player.MovementInfo.VelX = player.RunSpeed * signedHorizontal + speedModifier;

			//Handle any collision resitution & modify variables further if needed
			//TODO: Enemy Collision
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

			AnimationCheckSet();

			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, speedModifier);
			}

			//Handle Player input for state changers
			// && delta - PSDiveKick.last_bounce > 300)
			if (player.InputController.JumpPressed() && player.InputController.DownHeld())
				return new PSDiveKick(player);

			if (coyoteTimeTicks > 0)
			{
				--coyoteTimeTicks;
				if (player.InputController.JumpPressed())
				{
					return new PSJump(player, speedModifier);
				}
				if (player.Abilities.GroundDash &&
					player.InputController.DashPressed())
				{
					if (signedHorizontal != 0)
					{
						return new PSDash(player, signedHorizontal);
					}
				}
			}

			if (player.Abilities.DoubleJump &&
				player.InputController.JumpPressed() && 
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSJump(player, speedModifier);
			}

			if(signedHorizontal != 0 &&
				player.Abilities.AirDash && 
				player.InputController.DashPressed() && 
				player.Energy >= 1)
			{
				if (player.InputController.LeftStickHorizontal() != 0 || player.InputController.LeftStickVertical() != 0)
				{
					player.Energy -= 1;
					return new PSDash(player, signedHorizontal);
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
				player.Animation = "fall";
			else
				player.Animation = "jump";
		}
	}
}
