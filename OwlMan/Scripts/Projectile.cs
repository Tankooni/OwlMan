using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Godot;
using Godot.Collections;
using Atmo2.Enemy.AI;
using System;


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
		Damageable damageable;
		NodePath animatedSpritePath;
		AnimatedSprite2D projectileAnimatedSprite;
		bool isDeflected = false;

		private int lifeTicks = 0;
		private int lifeSpan = 120;

		public override void _Ready()
		{
			AddToGroup(HitGroups.Bullet);

			if(this.TargetHitgroups == null) {
				this.TargetHitgroups = new List<string>();
			}
			animatedSpritePath = GetPathTo(GetNode("AnimatedSprite2D"));
			projectileAnimatedSprite = GetNode<AnimatedSprite2D>(animatedSpritePath);			

			AddChild(movement = new AIVector(this, direction, speed));
			AddChild(damageable = new Damageable(this));
			damageable.OnDamageCallback += Deflect;
			
			// Connect(Damageable.SignalName.OnHandleDamageable, Callable.From<int>((damage) => HandleDamageDeflect(damage)));
			// damageable.OnHandleDamageable += HandleDamageDeflect;
			BodyEntered += OnCollide;
			AreaEntered += OnCollide;
		}

		public void OnCollide(Node2D otherNode2D)
		{
			// Tell the thing we collided with and our parent that we've collided if we actually did collide.
			// TODO: probably want to hook this up properly
			var damageable = otherNode2D.GetNodeOrNull<Damageable>(Damageable.DAMAGEABLE_NAME);
			if(damageable is not null)
			{
				damageable.HandleDamage(1);
			}
			QueueFree();
		/*
			if (body.IsInGroup(HitGroups.Enemy)) {
				if (isDeflected) {
					GD.Print("Bullet Hit Enemy and Will Damage Enemy");
					projectileAnimatedSprite.Frame = 2; // Color red
				} else {
					GD.Print("Bullet Hit Enemy and Will Not Damage Enemy");
					projectileAnimatedSprite.Frame = 0; // Color normal
				}
			} else if (body.IsInGroup(HitGroups.Player)) {
				if (isDeflected) {
					GD.Print("Bullet Hit Player and Will Not Damage Player");
					projectileAnimatedSprite.Frame = 1; // Color blue
				} else {
					GD.Print("Bullet Hit Player and Will Damage Player");
					projectileAnimatedSprite.Frame = 2; // Color red
				}
			}
		*/
		}

		public void Deflect(int damage, Damageable damageable)
		{
			if(isDeflected)
				return;
			
			lifeTicks = 0;
			isDeflected = true;
			movement.Direction = -direction;

			SetCollisionMaskValue(1, false);
			SetCollisionMaskValue(4, true);

			movement.Speed *= 2;
			Overlord.OwlOverlord.PlaySound("Hit4", GlobalPosition);
			    
			projectileAnimatedSprite.Frame = 1; // Color blue
		}

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
			lifeTicks++;
			if(lifeTicks >= lifeSpan)
			{
				QueueFree();
			}
        }
    }
}
