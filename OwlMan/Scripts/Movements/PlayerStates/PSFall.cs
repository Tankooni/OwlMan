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
		private float coyoteJumpSpeedModifier;

		public PSFall(Player player, float initialSpeedModifier = 0, bool coyoteTime = false, float speed = -1, float coyoteJumpSpeedModifier = 0)
			: base(player)
		{
			this.player = player;
			this.speed = speed < 0 ? player.RunSpeed : speed;
			this.coyoteTimeTicks = coyoteTime ? 10 : 0;
			this.coyoteJumpSpeedModifier = coyoteJumpSpeedModifier;
			speedModifier = initialSpeedModifier;
		}
		public override void OnEnter()
		{
			AnimationCheckSet();
		}

		public override void OnExit(PlayerState newState)
		{
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			player.MovementInfo.Velocity.Y += player.Gravity;
			if (player.MovementInfo.HeadBonk)
				player.MovementInfo.Velocity.Y = player.Gravity;
			
			// MOVEMENT --------------------------------------------------------------------------
			player.MovementInfo.Velocity.X = player.RunSpeed * signedHorizontal + speedModifier;
			
			if (speedModifier != 0)
			{
				var modSign = Math.Sign(speedModifier);

				speedModifier = speedModifier - player.HorizontalAirDrag * modSign;

				if(modSign != Math.Sign(speedModifier))
					speedModifier = 0;
			}
			// ---------------------------------------------------------------------------------

			if (signedHorizontal != 0)
				player.FacingDirection = signedHorizontal;
			else if ( speedModifier != 0)
				player.FacingDirection = Math.Sign(speedModifier);

			AnimationCheckSet();

			return CheckForNewState(signedHorizontal);
		}

        public PlayerState CheckForNewState(int signedHorizontal)
        {
			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, speedModifier);
			}

			//Handle Player input for state changers
			// && delta - PSDiveKick.last_bounce > 300)
			if (player.InputController.DashHeld() && player.InputController.DownHeld())
				return new PSDiveKick(player);

			if (coyoteTimeTicks > 0)
			{
				--coyoteTimeTicks;
				if (player.InputController.JumpPressed())
				{
					return new PSJump(player, speedModifier + coyoteJumpSpeedModifier);
				}
				if (player.Abilities.GroundDash &&
					player.InputController.DashPressed())
				{
					return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection);
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
				player.Energy -= 1;
				return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection);
			}

			if (player.MovementInfo.OnGround)
				if (player.MovementInfo.Velocity.X == 0)
					return new PSIdle(player);
				else // Hit the ground runnin'
					return new PSRun(player, initialSpeedModifier: speedModifier);

			// Determine if we're on a wall and only left or right is pressed for wall slide if enabled
			if (player.Abilities.WallSlide
				&& (player.MovementInfo.AgainstLeftWall || player.MovementInfo.AgainstRightWall)
				&& (player.InputController.LeftHeld() ^ player.InputController.RightHeld()))
			{
				if(player.MovementInfo.AgainstLeftWall && player.InputController.LeftHeld())
				{
					return new PSWallSlide(player, true);
				}
				
				if(player.MovementInfo.AgainstRightWall && player.InputController.RightHeld())
				{
					return new PSWallSlide(player, false);
				}
			}

			return null;
        }
        private void AnimationCheckSet()
		{
			if (player.MovementInfo.Velocity.Y > 0)
				player.Animation = "fall";
			else
				player.Animation = "jump";
		}
	}
}
