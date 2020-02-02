using Godot;
using Godot.Collections;
using Atmo2.Enemy.AI;

namespace Atmo2.Enemy {
  public class Bee : Enemy 
  {
    [Export]
    public NodePath Target { 
      get { return this.shootAI.Target; }
      set { this.shootAI.Target = value; } 
    }

    ShootAt shootAI;

    public override void _Ready()
    {
      base._Ready();

      this.shootAI = new ShootAt();
      this.shootAI.TargetHitgroups = new Array{HitGroups.Player, HitGroups.Wall};
      this.shootAI.Interval = 1.5f;
      this.shootAI.RandomInterval = 0.5f;

      this.AddChild(this.shootAI);
    }
  }
}