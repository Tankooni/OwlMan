using System;
using Godot;

namespace Atmo2
{
	public partial class Controller
	{
		private int jumpTicker { get; set; }

		public bool JumpPressedBuffered() { return jumpTicker > 0; }
		public bool JumpPressed() { return Input.IsActionJustPressed("jump"); }
		
		public void JumpSuccess() { jumpTicker = 0; }
		public bool DashPressed() { return Input.IsActionJustPressed("dash"); }
		public bool AttackPressed() { return Input.IsActionJustPressed("attack"); }
		public bool InteractPressed() { return Input.IsActionJustPressed("interact"); }
		
		public bool JumpHeld() { return Input.IsActionPressed("jump"); }
		public bool DashHeld() { return Input.IsActionPressed("dash"); }
		public bool AttackHeld() { return Input.IsActionPressed("attack"); }
		public bool InteractHeld() { return Input.IsActionPressed("interact"); }

		public bool UpHeld() { return Input.IsActionPressed("up"); }
		public bool DownHeld() { return Input.IsActionPressed("down"); }
		public bool LeftHeld() { return Input.IsActionPressed("left"); }
		public bool RightHeld() { return Input.IsActionPressed("right"); }

		public bool UpPressed() { return Input.IsActionJustPressed("up"); }
		public bool DownPressed() { return Input.IsActionJustPressed("down"); }
		public bool LeftPressed() { return Input.IsActionJustPressed("left"); }
		public bool RightPressed() { return Input.IsActionJustPressed("right"); }

		public Func<float> LeftStickHorizontal = () => { return Input.GetActionStrength("right") - Input.GetActionStrength("left"); };
		public Func<float> LeftStickVertical = () => { return Input.GetActionStrength("down") - Input.GetActionStrength("up"); };


		public Func<bool> Start = () => { return Input.IsActionJustPressed("start"); };
		public Func<bool> Select = () => { return Input.IsActionJustPressed("select"); };

		public void Update()
		{
			if (Input.IsActionJustPressed("jump"))
				jumpTicker = 10;
			else if (jumpTicker > 0)
				--jumpTicker;
		}
	}
}
