using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;
using Atmo2.Enemy.AI;


namespace Atmo2 {
  public partial class Projectile : Area2D
  {
		////[Export]
		public List<string> TargetHitgroups { get; set; }

		[Export]
		public Vector2 direction;

		[Export]
		public int speed;

		[Export]
		public AnimatedSprite2D projectileSprite;

		[Signal]
		public delegate void OnHitEventHandler(Node2D source, Node2D body);
		AIVector movement;

		public override void _Ready()
		{
			AddToGroup(HitGroups.Bullet);

			if(this.TargetHitgroups == null) {
				this.TargetHitgroups = new List<string>();
			}			

			movement = new AIVector(this, direction, speed);
			this.AddChild(movement);
			this.Connect("body_entered", new Callable(this, "OnCollide"));
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

			

			if (body.IsInGroup(HitGroups.Player)) {
				GD.Print("Body is a player");
			}
		
			if (body.IsInGroup(HitGroups.Player) && !isDeflected) {
						 // Get the path to the AnimatedSprite2D node
				NodePath animatedSpritePath = GetPathTo(GetNode("AnimatedSprite2D")); // Replace "AnimatedSprite" with the actual name of your AnimatedSprite2D node

				GD.Print("Is Player and is not deflected");
				// Check if the path is valid
				if (animatedSpritePath != null)
				{
					// Assuming your projectile has an AnimatedSprite2D as a child
					AnimatedSprite2D projectileAnimatedSprite = GetNode<AnimatedSprite2D>(animatedSpritePath);

					// Check if the AnimatedSprite2D node exists
					if (projectileAnimatedSprite != null)
					{
						// Change the current animation frame to 1
						projectileAnimatedSprite.Frame = 2;
					}
			}
			}
			
			// TODO: Pass an Attack object with a damage amount, pushback, damage type, etc instead of this object
			if(!body.IsInGroup(HitGroups.Enemy) || isDeflected) {
				// var parent = this.GetParent();
				// EmitSignal(nameof(OnHit), GetParent(), body);


				//TODO: need to fix collision layers
				//if(body.HasMethod("OnDamage"))
				//	body.Call("OnDamage", this);

				//QueueFree();
				//SetPhysicsProcess(false);

				GD.Print("OOF");
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

			 // Get the path to the AnimatedSprite2D node
    		NodePath animatedSpritePath = GetPathTo(GetNode("AnimatedSprite2D")); // Replace "AnimatedSprite" with the actual name of your AnimatedSprite2D node
			    
			// Check if the path is valid
			if (animatedSpritePath != null)
			{
				// Assuming your projectile has an AnimatedSprite2D as a child
				AnimatedSprite2D projectileAnimatedSprite = GetNode<AnimatedSprite2D>(animatedSpritePath);

				// Check if the AnimatedSprite2D node exists
				if (projectileAnimatedSprite != null)
				{
					// Change the current animation frame to 1
					projectileAnimatedSprite.Frame = 1;
				}
			}
  }
}
}
