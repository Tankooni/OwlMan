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

		Sprite2D.Play("run");
	}

	public void ChangeDirection(int direction)
	{
		if(direction < 0)
		{
			Sprite2D.FlipH = false;
		}
		else if(direction > 0)
		{
			Sprite2D.FlipH = true;
		}
	}
}
