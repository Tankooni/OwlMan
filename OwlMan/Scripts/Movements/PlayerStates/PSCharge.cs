using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Atmo2.Movements.PlayerStates
{
    class PSCharge : PlayerState
    {
        private float previous_charge_rate;
        private float charge_rate;
		//private GPUParticles2D poseParticles;

		private int ticker = 0;

        public PSCharge(Player player, float chargeRate=10)
			: base(player)
		{
            this.player = player;
            this.charge_rate = chargeRate;
			//poseParticles = player.GetNode<GPUParticles2D>("PoseParticles");
        }
		public override void OnEnter()
        {
            this.previous_charge_rate = player.EnergyRechargeRate;
            player.EnergyRechargeRate = charge_rate;

            // Animation
            player.Animation = "charge";
			//poseParticles.Emitting = true;

			// Sound
			//if (Engine.Random.Chance(1f))
			//AudioManager.PlaySoundVariations("charge2", .5f, .7f);
			//else
			// AudioManager.PlaySoundVariations("charge", .5f, .7f);
        }

        public override void OnExit()
        {
			//poseParticles.Emitting = false;
            player.EnergyRechargeRate = previous_charge_rate;
        }

        public override PlayerState Update()
        {
			
            player.RefillEnergy();

			if(ticker++ > 60)
			{
				ticker = 0;
				player.MovementInfo.StartShake = true;

			}
			ticker++;


			//TODO: Enemy Collision
            // Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            // if (enemy != null && !this.player.IsInvincable)
            // {
            //     return new PSOuch(player, enemy.touchDamage, KQ.STANDARD_GRAVITY);
            // }

            if (!player.InputController.DownHeld())
            {
                return new PSIdle(player);
            }
            return null;
        }
    }
}
