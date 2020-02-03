using Godot;

namespace Atmo2.Enemy.AI {
  /// This will move the given (or parent) entity in a straight line forever

  public class AIVector : Node2D
  {
    [Export]
    public Node2D Parent { get; set; }

    [Export]
    public Vector2 Direction = new Vector2(1, 0);

    [Export]
    public int Speed = 5;

    public override void _Ready()
    {
      this.SetPhysicsProcess(true);
    }

    public override void _PhysicsProcess(float delta)
    {
      // if(this.Parent == null) {
      //   this.Free();
      // }
      
      var motion = this.Direction * this.Speed;
      this.Parent.Position += motion;
    }
  }
}