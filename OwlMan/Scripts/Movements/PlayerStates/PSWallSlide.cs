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
			player.MovementInfo.VelX = 0;
			player.MovementInfo.VelY = 0;
			player.Image.FlipH = !wallOnLeft;

			player.ShakeCamera();

			player.Animation = "wallSlide";
		}

		public override void OnExit(PlayerState newState)
		{
			player.MovementInfo.VelX = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			player.MovementInfo.VelY += player.Gravity / 3;

			if(player.MovementInfo.OnGround)
			{
				return new PSIdle(player);
			}




			if(player.Abilities.WallJump && player.InputController.JumpPressed())
			{
				//TODO: Add some kind of directionality to this call
				return new PSJump(player, initialSpeedModifier: player.RunSpeed * 2);
			}

			// Check to see if we should enter into the falling state
			if(wallOnLeft)
			{
				if(!player.MovementInfo.AgainstLeftWall)
					return new PSFall(player);
				if(player.InputController.RightHeld())
					return new PSFall(player, coyoteTime: player.Abilities.WallJump, jumpSpeedModifier: player.RunSpeed * 2);
			}
			else 
			{
				if(!player.MovementInfo.AgainstRightWall)
					return new PSFall(player);
				if(player.InputController.LeftHeld())
					return new PSFall(player, coyoteTime: player.Abilities.WallJump, jumpSpeedModifier: player.RunSpeed * 2);
			}

			return null;
		}
	}
}
