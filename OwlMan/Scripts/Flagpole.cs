using Godot;
using System;
using System.Runtime.InteropServices;

public partial class Flagpole : Area2D
{

	// Signal to notify when the player touches the flag
    [Signal]
    public delegate void PlayerTouchedEventHandler();

    private CollisionShape2D _collisionShape2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		this.AreaEntered += OnArea2DAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnArea2DAreaEntered(Area2D otherArea)
	{
				// Assuming you have a reference to the AnimatedSprite2D node
		AnimatedSprite2D animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		// Change the animation to a new animation named "new_animation"
		animatedSprite.Play("owl");
	}

}

// 	// Called when the player enters the area of the flag
//     private void OnArea2DAreaEntered(Area2D otherArea)
//     {
// 		var typeInfo = otherArea.GetType();

// 		GD.Print("FLAGPOLE IS TOUCHED");
// 		GD.Print("Entering Area:", otherArea);
// 		GD.Print("Class:", typeInfo);

// 		        // Get the bodies that are overlapping with the area
//         var overlappingBodies = GetOverlappingBodies();

//         foreach (var body in overlappingBodies)
//         {
//             // Now you can access properties or methods of the overlapping body
//             GD.Print("Overlapping body:", body);
//         }

// 		if(otherArea.IsInGroup("Player")){
// 			GD.Print("FLAGPOLE IS TOUCHED BY PLAYER");
// 		}
//     }
// }


//         // // Check if the entering area is the Player node
//         // if (area is Player)
//         // {
//         //     // Emit the PlayerTouched signal
//         //     EmitSignal("PlayerTouched");
            
//         //     // You can add code here to swap the AnimatedSprite2D for the flag
//         //     // For example, change the animation or texture
//         //     // Example: Change the animation to "captured"
//         //     GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("captured");
//         // }