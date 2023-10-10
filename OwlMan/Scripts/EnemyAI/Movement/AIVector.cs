using Godot;

namespace Atmo2.Enemy.AI {
  /// This will move the given (or parent) entity in a straight line forever

  public partial class AIVector : Node2D
  {
    private Node2D parent;

    public Vector2 Direction;

    public int Speed;

    public AIVector(Node2D parent, Vector2 direction, int speed)
    {
      this.parent = parent;
      Direction = direction;
      Speed = speed;
    }
    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
      var motion = Direction * Speed;
      parent.Position += motion;
    }
  }
}