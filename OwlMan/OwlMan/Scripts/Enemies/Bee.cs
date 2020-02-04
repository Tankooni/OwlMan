using Godot;
using Godot.Collections;
using Atmo2.Enemy.AI;
using System.Collections.Generic;

namespace Atmo2.Enemy {
  public class Bee : Enemy 
  {
    [Export]
    public NodePath Target { 
      get { return this.shootAI.Target; }
      set { this.shootAI.Target = value; } 
    }

    [Export]
    public string AttackSoundName {get; set; }

    AnimatedSprite animatedSprite;
    ShootAt shootAI;

    public override void _Ready()
    {
      base._Ready();

      animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

      shootAI = new ShootAt(Shoot, ChangeDirection)
      {
        TargetHitgroups = new List<string> {HitGroups.Player, HitGroups.Wall}
      };

      this.AddChild(shootAI);
    }

    public void Shoot()
    {
      if(AttackSoundName != null && !AttackSoundName.Empty())
        Overlord.OwlOverlord.PlaySound(AttackSoundName, this.GlobalPosition);
      animatedSprite.Play("attack");
      animatedSprite.Connect("animation_finished", animatedSprite, "play", new Array {"idle"}, (uint)ConnectFlags.Oneshot);
    }

    public void ChangeDirection(Vector2 direction)
    {
      if(direction.x < 0)
			  animatedSprite.FlipH = false;
      else if(direction.x > 0)
			  animatedSprite.FlipH = true;
    }
  }
}