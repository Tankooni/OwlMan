using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;
using Atmo2.Enemy.AI;


namespace Atmo2 {
  public class Projectile : Area2D
  {
		[Export]
		public Array TargetHitgroups { get; set; }

		[Export]
		public Vector2 direction;

		[Export]
		public int speed;

		[Signal]
		public delegate void OnHit(Node2D source, Node2D body);
		AIVector movement;

		public override void _Ready()
		{
			AddToGroup(HitGroups.Bullet);

			if(this.TargetHitgroups == null) {
				this.TargetHitgroups = new Array();
			}			

			movement = new AIVector
			{
				Parent = this,
				Speed = speed,
				Direction = direction
			};
			this.AddChild(movement);
			this.Connect("body_entered", this, "OnCollide");
		}
		
		bool isDeflected = false;

		public void OnCollide(Node body)
		{
			// Tell the thing we collided with and our parent that we've collided if we actually did collide
			// bool isInGroup = false; // Use Intersect(body) when godot makes it's arrays IEnumerable
			
			// foreach(string g in body.GetGroups() {
			// 	if(HitGroups.Player == )
			// 	if(this.TargetHitgroups.Contains(g)) {
			// 		isInGroup = true;
			// 		GD.Print(g);
			// 		break;
			// 	}
			// }
			
			// TODO: Pass an Attack object with a damage amount, pushback, damage type, etc instead of this object
			if(!body.IsInGroup(HitGroups.Enemy) || isDeflected) {
				// var parent = this.GetParent();
				// EmitSignal(nameof(OnHit), GetParent(), body);

				if(body.HasMethod("OnDamage"))
					body.Call("OnDamage", this);
				
				QueueFree();
				SetPhysicsProcess(false);
			}
		}

		public void Deflect()
		{
			if(isDeflected)
				return;
				
			isDeflected = true;
			movement.Direction = -direction;
			movement.Speed *= 2;
			Overlord.OwlOverlord.PlaySound("Hit4", GlobalPosition);
		}
  }
}
