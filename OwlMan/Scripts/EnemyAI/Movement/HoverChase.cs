using System;
using System.Collections.Generic;
using Godot;

namespace Atmo2.Enemy.AI {
    public partial class HoverChase : Node2D
    {
        public NodePath Target { 
            get { return this.target?.GetPath(); }
            set { 
                var n = GetNode<Node2D>(value);
                if( n != null) this.target = n; 
            }
        }
        Node2D target;

        private CharacterBody2D parent;
        private int speed;
        private int activeDistance;
        private int tooCloseDistance;

        public HoverChase(CharacterBody2D parent, int speed, int activeDistance = 300, int tooCloseDistance = 250)
        {
            this.parent = parent;
            this.speed = speed;
            this.activeDistance = activeDistance;
            this.tooCloseDistance = tooCloseDistance;
        }

        public override void _Ready()
        {
            Target = Enemy.PlayerPath;
        }

        public override void _PhysicsProcess(double delta)
        {
            var distance = GlobalPosition.DistanceTo(target.GlobalPosition);
            if(distance > activeDistance + 100)
                return;
            
            var direction = GlobalPosition.DirectionTo(target.GlobalPosition);

            if (distance > 300)
                parent.Velocity = direction * speed;
            else if(distance < 250)
				parent.Velocity = -direction * speed;
			parent.MoveAndSlide();
		}
    }
}