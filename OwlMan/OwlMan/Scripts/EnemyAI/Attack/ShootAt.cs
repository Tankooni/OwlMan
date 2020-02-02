using Godot;
using Godot.Collections;

namespace Atmo2.Enemy.AI {
  class ShootAt : Node2D
  {
    PackedScene bullet = ResourceLoader.Load<PackedScene>("res:///prefab/Projectiles/Bullet.tscn");

    [Export]
    public NodePath Target { 
      get { return this.target?.GetPath(); }
      set { 
        var n = GetNode<Node2D>(value);
        if( n != null) this.target = n; 
      } 
    }
    Node2D target;

    [Export]
    public Array TargetHitgroups { get; set; }

    [Export]
    public float Interval { get; set; }
    [Export]
    public float RandomInterval { get; set; }

    Timer timer;
    Animation last_animation;

    public override void _Ready()
    {
      GD.Print("shoot at start");
      // Default to target's groups if none are given
      if(this.TargetHitgroups == null) {
        this.TargetHitgroups = target.GetGroups();
      }

      this.timer = new Timer();

      this.timer.WaitTime = this.Interval;
      this.timer.OneShot = false;
      this.timer.Connect("timeout", this, "Shoot");
      this.timer.Autostart = true;

      this.AddChild(this.timer);
    }

    void Shoot()
    {
      // Set the timer again
      float r = (GD.Randf() * this.RandomInterval * 2) - this.RandomInterval;
      this.timer.WaitTime = this.Interval + r;
      //this.timer.Start();

      // Shoot at the player if we have nothing to shoot at
      if(true) { //this.Target == null) {
        this.Target = Enemy.PlayerPath;
      }

      Node b = bullet.Instance();
      GD.Print(b);
      var direction = this.GetParent<Node2D>().Position.DirectionTo(this.target.Position);

      b.Set("direction", direction);
      b.Set("TargetHitgroups", this.TargetHitgroups);
      b.Set("speed", 5);
      b.Set("Position", this.Position);
      //b.direction = direction;
      //b.TargetHitgroups = this.TargetHitgroups;
      //b.speed = 5;
      //b.Position = this.Position;
      GD.Print("FUCK");
      GetTree().Root.AddChild(b);
    }
  }
}