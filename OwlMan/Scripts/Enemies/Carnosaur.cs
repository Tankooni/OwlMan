using Atmo2.Enemy;
using Atmo2.Enemy.AI;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class Carnosaur : Enemy
{
    [Export]
    public string AttackSoundName {get; set; }

    AnimatedSprite animatedSprite;
    ShootAt shootAI;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        shootAI = new ShootAt(Shoot, ChangeDirection, 120)
        {
        TargetHitgroups = new List<string> {HitGroups.Player, HitGroups.Wall}
        };

        AddChild(shootAI);
        AddChild(new HoverChase(this, 150, 300, 250));
    }

    public void Shoot()
    {
      if(AttackSoundName != null && !AttackSoundName.Empty())
        Overlord.OwlOverlord.PlaySound(AttackSoundName, this.GlobalPosition);
      animatedSprite.Play("attack");
      animatedSprite.Connect("animation_finished", animatedSprite, "play", new Godot.Collections.Array { "idle"}, (uint)ConnectFlags.Oneshot);
    }

    public void ChangeDirection(Vector2 direction)
    {
      if(direction.x < 0)
		animatedSprite.FlipH = false;
      else if(direction.x > 0)
		animatedSprite.FlipH = true;
    }
}
