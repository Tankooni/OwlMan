using Burden.DebugDrawing;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
    class PSDash : PlayerState
    {
        private float speed = 0;
        private float maxSpeed;
        private int dashTicks;
        private bool coyoteTime;
        private int coyoteTicks;
		private int inAirTicks;
		private float direction;

		private bool coyotedOnFloor;

		private float SpeedModifier { get { return player.DashMultiplier * player.RunSpeed * direction; } }

/// <summary>
/// 
/// </summary>
/// <param name="player"></param>
/// <param name="direction"></param>
/// <param name="dashTicks">Should probably be around 10 ticks or 1/6 of a second</param>
/// <param name="coyoteTime">Should we have coyote time for jumps while "in the air"</param>
        public PSDash(Player player, float direction, bool coyoteTime = false, int dashTicks = 15)
			: base(player)
		{
            this.player = player;
            this.maxSpeed = player.DashMultiplier * player.RunSpeed;
			this.direction = direction;
            this.dashTicks = dashTicks;
			this.coyoteTime = coyoteTime;
			this.coyoteTicks = coyoteTime ? 7 : 0;
			this.inAirTicks = 0;

			// speed = MathF.Min(player.RunSpeed * 4, maxSpeed);
			player.MovementInfo.Velocity.X = maxSpeed * direction;
			player.MovementInfo.Velocity.Y = 0;

			player.FacingDirection = direction;
		}
        public override void OnEnter()
        {
            player.Animation = "dash";
			//direction = new Vector2(player.InputController.LeftStickHorizontal(), 0/*Controller.LeftStickVertical()*/).Normalized();
		}

        public override void OnExit(PlayerState newState)
		{
			// player.MovementInfo.ResetBoxes();
		}

        public override PlayerState Update()
        {
			var signedHorizontal = Math.Sign(player.InputController.LeftStickHorizontal());

			player.MovementInfo.Velocity.X = maxSpeed * direction;
			coyotedOnFloor = player.IsOnFloor();
			if( !coyotedOnFloor )
			{
				++inAirTicks;

				if( inAirTicks <= coyoteTicks )
				{
					coyotedOnFloor = true;
				}
			}
			else if ( inAirTicks != 0 ) // If we're on the floor, reset air tick counter if needed
			{
				inAirTicks = 0;
			}

			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, SpeedModifier);
			}

			// if (player.InputController.DashHeld()
			// &&	player.InputController.DownHeld()
			// &&	!player.IsOnFloor())
			// {
			// 	return new PSDiveKick(player);
			// }

			if (coyotedOnFloor)
			{
				if( player.InputController.JumpPressedBuffered() )
				{
					var extraJumpMult = 1f;
					var speedModMult = 1f;

					if ( player.IsOnFloor() )
					{
						float percentMaxAngle = player.GetFloorAngle() / (Mathf.Pi / 4);
						extraJumpMult += .3f * percentMaxAngle;
						speedModMult -= .4f * percentMaxAngle;
					}
					
					return new PSJump(player, SpeedModifier * speedModMult, extraJumpMult);
				}
			}
			else
			{
				if( player.InputController.JumpPressedBuffered() )
				{
					if (	player.Abilities.DoubleJump
						&&	player.Energy >= 1)
					{
						player.Energy -= 1;
						return new PSJump(player, SpeedModifier);
					}
				}
			}

			if (player.IsOnWallOnly())
			{
				// TODO: add directional check in case we're flush on a wall but moving away
				if(player.Abilities.WallSlide)
					return new PSWallSlide(player, player.GetWallNormal().X > 0);
			}

			// if( player.Abilities.AirDash && 
			// 	player.InputController.DashPressed() && 
			// 	player.Energy >= 1)
			// {
			// 	player.Energy -= 1;
			// 	return new PSDash(player, signedHorizontal != 0 ? signedHorizontal : player.FacingDirection);
			// }


			if(		player.IsOnWall()
				||	player.InputController.JumpPressed() 
				||	player.InputController.DashPressed()
				||	signedHorizontal != 0 
					&&	signedHorizontal != direction )
			{
				dashTicks = 0;
			}

			--dashTicks;
            if( dashTicks < 0 )
            {
				// We're done here
				if (player.IsOnFloor())
				{
					if( !player.IsOnWall() || signedHorizontal != 0)
					{
						return new PSRun(player, SpeedModifier);
					}
					else
					{
						return new PSIdle(player);
					}
				}
				else
				{
					return new PSFall(player, SpeedModifier);
				}
            }
            return null;
        }
    }
}
