using System;
using System.Collections.Generic;
using Godot;

namespace Atmo2.Enemy.AI {
    public partial class ChaseDown : Node2D
    {
		private Node2D target;
        public bool IsChasing { get; set; }
		private Node2D Target
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

        public ChaseDown(CharacterBody2D parent, int speed, int activeDistance = 300)
        {
            this.parent = parent;
            this.speed = speed;
            this.activeDistance = activeDistance;
        }

        public override void _Ready()
        {
            base._Ready();
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

			if(Target == null)
			{ 
				return;
			}

            if( IsChasing )
            {
                parent.Velocity = GlobalPosition.DirectionTo(Target.GlobalPosition) * speed ;
                parent.MoveAndSlide();
                return;
            }

            if ( !IsChasing && GlobalPosition.DistanceTo(Target.GlobalPosition) < activeDistance )
            {
                IsChasing = true;
            }
		}
    }
}