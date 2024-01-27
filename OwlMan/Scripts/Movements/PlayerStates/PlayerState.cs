using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atmo2.Movements.PlayerStates
{
    public abstract class PlayerState
    {
		protected Player player;

		public PlayerState(Player player)
		{
			this.player = player;
		}

		public abstract void OnEnter();
		public abstract PlayerState Update();
		public abstract void OnExit(PlayerState newState);
		public virtual PlayerState OnAnimationComplete()
		{
			return null;// new PSIdle(player);
		}
	}
}
