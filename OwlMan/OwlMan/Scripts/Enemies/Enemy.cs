using Godot;
using Atmo2.Enemy.AI;
using System.Collections.Generic;

namespace Atmo2.Enemy {
  public abstract class Enemy : KinematicBody2D
  {
    // So this is maybe not the best design but it is by far the most convenient and not as hacky solution. This should eventually be replaced with some sort of AI player detection system.
    public static NodePath PlayerPath;
    [Export]
    public int Health = 1;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
      
    }

    public void OnDamage(CollisionObject2D collider)
    {
      this.Health -= 1;
      if(this.Health <= 0) {
        this.QueueFree();
        SetPhysicsProcess(false);
      }
    }
  }
}