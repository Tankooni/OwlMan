using System;
using Godot;

namespace Atmo2
{
	public class Controller
	{
		public static Func<bool> JumpPressed = () => { return Input.IsActionJustPressed("jump"); };
		public static Func<bool> DashPressed = () => { return Input.IsActionJustPressed("dash"); };
		public static Func<bool> AttackPressed = () => { return Input.IsActionJustPressed("attack"); };

		public static Func<bool> JumpHeld = () => { return Input.IsActionPressed("jump"); };
		public static Func<bool> DashHeld = () => { return Input.IsActionPressed("dash"); };
		public static Func<bool> Attackv = () => { return Input.IsActionPressed("attack"); };

		public static Func<bool> UpHeld = () => { return Input.IsActionPressed("up"); };
		public static Func<bool> DownHeld = () => { return Input.IsActionPressed("down"); };
		public static Func<bool> LeftHeld = () => { return Input.IsActionPressed("left"); };
		public static Func<bool> RightHeld = () => { return Input.IsActionPressed("right"); };

		public static Func<bool> UpPressed = () => { return Input.IsActionJustPressed("up"); };
		public static Func<bool> DownPressed = () => { return Input.IsActionJustPressed("down"); };
		public static Func<bool> LeftPressed = () => { return Input.IsActionJustPressed("left"); };
		public static Func<bool> RightPressed = () => { return Input.IsActionJustPressed("right"); };

		public static Func<float> LeftStickHorizontal = () => { return Input.GetActionStrength("right") - Input.GetActionStrength("left"); };
		public static Func<float> LeftStickVertical = () => { return Input.GetActionStrength("down") - Input.GetActionStrength("up"); };


		public static Func<bool> Start = () => { return Input.IsActionJustPressed("start"); };
		public static Func<bool> Select = () => { return Input.IsActionJustPressed("select"); };
	}
}
