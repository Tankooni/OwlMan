using Godot;
using System;

public class DrawBox : StaticBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	RectangleShape2D box;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		box = GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
	}

	public override void _Draw()
	{
		// Your draw commands here
		DrawRect(new Rect2(-box.Extents.x, -box.Extents.y, box.Extents.x*2, box.Extents.y*2), new Color(1, 0, 0, 1));
	}

	public override void _Process(float delta)
	{
		Update();
	}
}
