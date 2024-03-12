using System;
using System.Collections.Generic;
using Godot;

namespace Atmo2.Enemy.AI {
    public partial class HoverChase : Node2D
    {
		private Node2D target;
		public Node2D Target
		{
			get
			{	
				if(target == null)
				{
					if( Enemy.PlayerPath == null)
						return null;
					target = GetTree().CurrentScene.GetNodeOrNull<Targetable>( $"{Enemy.PlayerPath.ToString()}/{nameof(Targetable)}" );
				}
				return target ??= GetTree().CurrentScene.GetNodeOrNull<Node2D>( Enemy.PlayerPath );
			}
		}

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
            
        }

        public override void _PhysicsProcess(double delta)
        {
			if(Target == null)
			{ 
				return;
			}

            var distance = GlobalPosition.DistanceTo(Target.GlobalPosition);
            if(distance > activeDistance + 100)
                return;
            
            var direction = GlobalPosition.DirectionTo(Target.GlobalPosition);

            if (distance > 300)
                parent.Velocity = direction * speed;
            else if(distance < 250)
				parent.Velocity = -direction * speed;
			parent.MoveAndSlide();
		}
    }
}