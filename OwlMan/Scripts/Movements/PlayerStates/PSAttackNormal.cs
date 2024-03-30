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
		private float prevDirection = 0;
		private AttackDirection atkDir;
		private float gravityScale = 1;
		private int downHeldTicks;

		private bool hasBounced = false;

		public PSAttackNormal(Player player, float initialSpeedModifier)
			: base(player)
		{
			this.player = player;
			speedModifier = initialSpeedModifier;
		}

		public override void OnEnter()
		{
			player.Animation = "attackNormal";
			
			ResolveAttackDirection();
		}

		public override void OnExit(PlayerState newState)
		{
			player.MovementInfo.ResetBoxes();
			player.Image.RotationDegrees = 0;
		}

		public override PlayerState Update()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			// Start the player back downward if we bonk our head
			if (player.IsOnCeiling() && player.MovementInfo.Velocity.Y < 0)
			{
				player.MovementInfo.Velocity.Y = 0;
			}

			if (!player.IsOnFloor())
			{
				player.MovementInfo.Velocity.Y += player.Gravity;
			}

			// Slightly float the player mid-fall
			gravityScale = Mathf.Abs(player.MovementInfo.Velocity.Y) > 110 && !player.InputController.JumpHeld() ? 1 : .65f;

			// if( player.InputController.DownHeld() )
			// {
			// 	if( ++downHeldTicks > 5 )
			// 	{
			// 		gravityScale += .5f;
			// 	}
			// }
			// else if ( downHeldTicks > 0 )
			// {
			// 	downHeldTicks = 0;
			// }

			player.MovementInfo.Velocity.Y += player.Gravity * gravityScale;

			if( !hasBounced && player.HitBounceDirection.Length() > 0 ) 
			{
				hasBounced = true;
				++player.Energy;
				player.MovementInfo.Velocity.Y = -player.JumpStrenth * 1.3f;
			}
			
			// MOVEMENT --------------------------------------------------------------------------
			player.MovementInfo.Velocity.X = player.RunSpeed * signedHorizontal + speedModifier;
			
			if (speedModifier != 0)
			{
				var modSign = Math.Sign(speedModifier);

				speedModifier = speedModifier - player.HorizontalGroundDrag * modSign;

				if(modSign != Math.Sign(speedModifier))
					speedModifier = 0;
			}
			// ---------------------------------------------------------------------------------
			
			return null;
		}

		private void ResolveAttackDirection()
		{
			//Collect variables to run calculations on
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());
			var signedVertical = Math.Sign(player.InputController.LeftStickVertical());
			
			player.MovementInfo.ResetBoxes();

			if ( signedVertical > 0 )
			{
				atkDir = AttackDirection.Down;
				player.FacingDirection = 1;
				player.Image.RotationDegrees = 90;

				player.MovementInfo.DownTrace = true;
			}
			else if ( signedVertical < 0 )
			{
				atkDir = AttackDirection.Up;
				player.FacingDirection = 1;
				player.Image.RotationDegrees = -90;

				player.MovementInfo.UpTrace = true;
			}
			else if (signedHorizontal < 0)
			{
				player.FacingDirection = signedHorizontal;
				if (signedHorizontal < 0)
				{
					atkDir = AttackDirection.Left;
					player.MovementInfo.LeftTrace = true;
				}
				else
				{
					atkDir = AttackDirection.Right;
					player.MovementInfo.RightTrace = true;
				}
			}
			else
			{	
				if (player.FacingDirection < 0)
				{
					atkDir = AttackDirection.Left;
					player.MovementInfo.LeftTrace = true;
				}
				else
				{
					atkDir = AttackDirection.Right;
					player.MovementInfo.RightTrace = true;
				}
			}
		}

        public override PlayerState OnAnimationComplete()
		{
			if (player.IsOnFloor())
				if (player.InputController.LeftHeld() || player.InputController.RightHeld())
					return new PSRun(player, initialSpeedModifier: speedModifier);
				else
					return new PSIdle(player);
			else
				return new PSFall(player, initialSpeedModifier: speedModifier);
		}
	}

	enum AttackDirection
	{
		Left,
		Right,
		Up,
		Down
	}
}
