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
		private float direction;

		private float SpeedModifier { get { return player.DashMultiplier * player.RunSpeed * direction; } }

        public PSDash(Player player, float direction, int dashTicks = 10) //dash should probably be 9-10
			: base(player)
		{
            this.player = player;
            this.maxSpeed = player.DashMultiplier * player.RunSpeed;
			this.direction = direction;
            this.dashTicks = dashTicks;

			// speed = MathF.Min(player.RunSpeed * 4, maxSpeed);
			player.MovementInfo.Velocity.X = maxSpeed * direction;
			player.MovementInfo.Velocity.Y = 0;
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
			// speed = MathF.Min(speed + speed * 1.2f, maxSpeed);
			player.MovementInfo.Velocity.X = maxSpeed * direction;

			if (player.InputController.AttackPressed())
			{
				return new PSAttackNormal(player, SpeedModifier);
			}

			if (!player.MovementInfo.OnGround)
			{
				// && delta - PSDiveKick.last_bounce > 300)
				if (player.InputController.DashHeld() && player.InputController.DownHeld())
					return new PSDiveKick(player);

				if (player.Abilities.DoubleJump &&
					player.InputController.JumpPressedBuffered() &&
					player.Energy >= 1)
				{
					player.Energy -= 1;
					return new PSJump(player, SpeedModifier);
				}
			}

			if (player.MovementInfo.OnGround && player.InputController.JumpPressedBuffered())
			{
				return new PSJump(player, SpeedModifier);
			}

			if (player.MovementInfo.AgainstLeftWall || player.MovementInfo.AgainstRightWall)
			{
				if(player.MovementInfo.AgainstLeftWall && player.InputController.LeftPressed())
				{
					
				}
				else if(player.MovementInfo.AgainstRightWall && player.InputController.RightPressed())
				{
					
				}


				// TODO: add directional check in case we're flush on a wall but moving away
				if(player.Abilities.WallSlide)
					return new PSWallSlide(player, player.MovementInfo.AgainstLeftWall);
				
				// Our dash should be considered done since we've hit a wall
				dashTicks = 0;
			}

			--dashTicks;
            if(dashTicks < 0)
            {
				// We're done here
				if (player.MovementInfo.OnGround)
					// if (player.InputController.LeftHeld() || player.InputController.RightHeld())
					return new PSRun(player, SpeedModifier);
					// else
					// 	return new PSIdle(player);
				else
				{
					return new PSFall(player, SpeedModifier);
				}
            }
            return null;
        }
    }
}
