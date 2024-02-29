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
			player.MovementInfo.Velocity.X = 0;
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
			// if ( player.MovementInfo.Velocity.Y >= player.Gravity * 10f )
			// {
			// 	player.MovementInfo.Velocity.Y = Mathf.Min(player.MovementInfo.Velocity.Y + player.Gravity / 3, player.Gravity * 10f);
			// }
			// else 
			// {
			// 	player.MovementInfo.Velocity.Y = Mathf.Min(player.MovementInfo.Velocity.Y + player.Gravity, player.Gravity * 10f);
			// }

			

			if(player.Abilities.WallJump && player.InputController.JumpPressed())
			{
				//TODO: Add some kind of directionality to this call
				return new PSJump(player, initialSpeedModifier: player.RunSpeed * 3 * (wallOnLeft ? 1 : -1) );
			}
			
			if ( player.MovementInfo.OnGround && ( player.InputController.LeftHeld() || player.InputController.RightHeld() ) )
			{
				return new PSRun(player);
			}

			// Check to see if we should enter into the falling state
			if(wallOnLeft)
			{
				if(!player.MovementInfo.AgainstLeftWall)
					return new PSFall(player);
				if(player.InputController.RightHeld())
					return new PSFall(player, coyoteTime: player.Abilities.WallJump, coyoteJumpSpeedModifier: player.RunSpeed * 3);
			}
			else 
			{
				if(!player.MovementInfo.AgainstRightWall)
					return new PSFall(player);
				if(player.InputController.LeftHeld())
					return new PSFall(player, coyoteTime: player.Abilities.WallJump, coyoteJumpSpeedModifier: -player.RunSpeed * 3);
			}
			
			if(player.MovementInfo.OnGround)
			{
				return new PSIdle(player);
			}

			if (player.Abilities.AirDash &&
				player.InputController.DashPressed() &&
				player.Energy >= 1)
			{
				player.Energy -= 1;
				return new PSDash(player, player.FacingDirection);
			}

			return null;
		}
	}
}
