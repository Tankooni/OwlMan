using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
	class PSWallSlide : PlayerState
	{
		private bool wallOnLeft = false;
		public PSWallSlide(Player player, bool wallOnLeft)
			: base(player)
		{
			this.player = player;
			this.wallOnLeft = wallOnLeft;
		}
		public override void OnEnter()
		{
			// Hack to make sure we continue to push against the wall
			player.MovementInfo.Velocity.X = wallOnLeft ? -1 : 1;
			// if(player.MovementInfo.Velocity.Y > 0)
			// {
			// 	player.MovementInfo.Velocity.Y = 0;
			// }
			player.FacingDirection = wallOnLeft ? 1 : -1;

			// player.ShakeCamera();

			player.Animation = "wallSlide";
		}

		public override void OnExit(PlayerState newState)
		{
			// player.MovementInfo.NewVel.X = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			// Apply gravity but once we're at a certain rate, keep it constant. This allows for upward momentum 
			player.MovementInfo.Velocity.Y = Mathf.Min(player.MovementInfo.Velocity.Y + player.Gravity * 1.5f, player.Gravity * 5f);		
				

			if(player.Abilities.WallJump && player.InputController.JumpPressedBuffered())
			{
				//TODO: Add some kind of directionality to this call
				return new PSJump(player, initialSpeedModifier: player.RunSpeed * 3 * (wallOnLeft ? 1 : -1) );
			}

			if ( player.IsOnFloor() )
			{
				if (player.Abilities.GroundDash &&
					player.InputController.DashPressed())
				{
					return new PSDash(player, player.FacingDirection);
				}
				if ( player.InputController.LeftHeld() || player.InputController.RightHeld() )
				{
					return new PSRun(player);
				}
				return new PSIdle(player);
			}

			if(!player.IsOnWallOnly())
			{
				return new PSFall(player);
			}

			if (player.Abilities.AirDash &&
				player.InputController.DashPressed() &&
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSDash(player, player.FacingDirection);
			}

			if(wallOnLeft)
			{
				if(player.InputController.RightHeld())
				{
					return new PSFall(player, coyoteTime: player.Abilities.WallJump, coyoteJumpSpeedModifier: player.RunSpeed * 3);
				}
			}
			else 
			{
				if(player.InputController.LeftHeld())
				{
					return new PSFall(player, coyoteTime: player.Abilities.WallJump, coyoteJumpSpeedModifier: -player.RunSpeed * 3);
				}
			}

			return null;
		}
	}
}
