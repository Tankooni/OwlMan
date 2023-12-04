using Atmo2.Enemy;
using Godot;
using System;

public partial class Bug : Enemy
{
    private AnimatedSprite2D animatedSprite;


    public override void _Ready()
    {
        base._Ready();

        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        AddToGroup(HitGroups.Enemy);
        
        AddChild(new WalkTurn(ChangeDirection, this, 200));
    }

    public void ChangeDirection(int direction)
    {
      if(direction < 0)
	    animatedSprite.FlipH = false;
      else if(direction > 0)
		animatedSprite.FlipH = true;
    }
}
