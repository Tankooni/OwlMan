using Atmo2.Enemy;
using Godot;
using System;

public partial class Bug : Enemy
{
	[Export]
	public WalkTurn WalkTurn;
	public override void _Ready()
	{
		base._Ready();
		
		// AddChild(new WalkTurn(ChangeDirection, this, 200));
		
		WalkTurn.changeDirection += ChangeDirection;

		AnimPlayer.Play("Run");
	}

	public void ChangeDirection(int direction)
	{
		if(direction < 0)
		{
			Sprite.FlipH = false;
		}
		else if(direction > 0)
		{
			Sprite.FlipH = true;
		}
	}
}
