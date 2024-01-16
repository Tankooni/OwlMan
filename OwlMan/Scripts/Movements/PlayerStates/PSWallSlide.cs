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
		private bool isLeft = false;
		public PSWallSlide(Player player, bool isLeft = false)
			: base(player)
		{
			this.player = player;
			this.isLeft = isLeft;
		}
		public override void OnEnter()
		{
			player.MovementInfo.VelX = 0;
			player.Image.FlipH = isLeft;

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

			player.MovementInfo.VelY += player.Gravity / 2;

			AnimationCheckSet();

			if(player.MovementInfo.OnGround)
			{
				return new PSIdle(player);
			}

			if(isLeft && player.MovementInfo.AgainstWall > -1)
			{
				return new PSFall(player);
			}
			else if (player.MovementInfo.AgainstWall < 1)
			{
				return new PSFall(player);
			}

			if(player.InputController.LeftHeld() ^player.InputController.RightHeld())
			{
				// if(isLeft && )
				// {
					
				// }
			}

			return null;
		}
		private void AnimationCheckSet()
		{
			if (player.MovementInfo.VelY > 0)
				player.Animation = "wallSlide";
			else
				player.Animation = "wallSlide";
		}
	}
}
