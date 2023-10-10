using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Movements.PlayerStates;

namespace Atmo2.Movements
{
	public partial class PlayerStateController
	{
		public PlayerState current_state;
		private PlayerState next_state;

		public PlayerStateController(PlayerState initial_state)
		{
			this.current_state = initial_state;
			initial_state.OnEnter();
		}

		public void Update()
		{
			var newState = current_state.Update();

			// Transision states if needed
			if (next_state != null)
			{
				newState = next_state;
				next_state = null;
			}

			if(newState != null)
			{
				current_state.OnExit();
				newState.OnEnter();
				current_state = newState;
			}
		}

		public void NextState(PlayerState state)
		{
			next_state = state;
		}

		public void AnimationComplete()
		{
			var next = current_state.OnAnimationComplete();
			if (next != null)
				next_state = next;
		}
	}
}
