using System;
using System.Collections.Generic;
using Godot;

namespace Atmo2.Enemy.AI {
    public class HoverChase : Node2D
    {
        public NodePath Target { 
            get { return this.target?.GetPath(); }
            set { 
                var n = GetNode<Node2D>(value);
                if( n != null) this.target = n; 
            }
        }
        Node2D target;

        private KinematicBody2D parent;
        private int speed;
        private int activeDistance;
        private int tooCloseDistance;

        public HoverChase(KinematicBody2D parent, int speed, int activeDistance = 300, int tooCloseDistance = 250)
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

        public override void _PhysicsProcess(float delta)
        {
            var distance = GlobalPosition.DistanceTo(target.GlobalPosition);
            if(distance > activeDistance + 100)
                return;
            
            var direction = GlobalPosition.DirectionTo(target.GlobalPosition);
            
            if(distance > 300)
                parent.MoveAndSlide(direction * speed);
            else if(distance < 250)
                parent.MoveAndSlide(-direction * speed);
        }
    }
}