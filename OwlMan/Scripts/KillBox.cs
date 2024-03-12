using Godot;
using System;

[GlobalClass]
public partial class KillBox : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
		public Action<Node2D> HitCallback;

    public int damageAmount = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
      AreaEntered += OnAreaEntered;
      BodyEntered += OnBodyEntered;
      Monitoring = false;
      
      // Connect("", new Callable(GetParent(), ""));
	  }


    public void SetTraceState( bool IsOn, int damage = 1 )
    {
      if( IsOn == Monitoring )
      {
        return;
      }

      damageAmount = damage;

      if(IsOn)
      {
        Monitoring = true;
        // ManualBodyCheck();
      }
      else
      {
        Monitoring = false;
      }
    }

    public void OnAreaEntered(Area2D area2D)
    {
      // GD.Print($"Area Entered, {area2D.Name}.");
      ApplyDamage(area2D);
    }

    public void ManualBodyCheck()
    {
        foreach(var body in GetOverlappingBodies())
        {
          OnBodyEntered(body);
        }
    }

    private void OnBodyEntered(Node2D node2D)
    {
      // GD.Print($"Body Entered, {node2D.Name}");
      ApplyDamage(node2D);
    }

    private void ApplyDamage(Node2D otherNode2D)
    {
      var damageable = otherNode2D.GetNodeOrNull<Damageable>(Damageable.DAMAGEABLE_NAME);
      if(damageable is not null)
      {
        damageable.HandleDamage(damageAmount);
        if(HitCallback is not null)
          HitCallback(otherNode2D);
      }
      // damageable.EmitSignal(Damageable.SignalName.OnHandleDamageable, damageAmount);
      // if(otherNode2D.HasUserSignal(Damageable.SignalName.OnHandleDamageable))
      // {
      //   GD.Print($"No damage signal emit {Damageable.SignalName.OnHandleDamageable}");
        
      //   otherNode2D.EmitSignal(Damageable.SignalName.OnHandleDamageable, damageAmount);
      // }
      // else
      // {
      //   otherNode2D.PropagateCall(Damageable.SignalName.OnHandleDamageable, new Godot.Collections.Array(){damageAmount}, true);
      //   GD.Print($"No damage signal found {Damageable.SignalName.OnHandleDamageable}");
      // }

      
      // otherNode2D.QueueFree();
    }

}