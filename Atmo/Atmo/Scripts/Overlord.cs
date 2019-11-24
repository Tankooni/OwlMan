using Godot;
using System;
using Atmo.OgmoLoader;

public class Overlord : Node
{
	public static float STANDARD_GRAVITY = 0f;

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        var ogmo = new OgmoLoader();
		var yeah = ogmo.Load();

		Viewport root = GetTree().GetRoot();
		var CurrentScene = root.GetChild(root.GetChildCount() - 1);
		CurrentScene.AddChild(yeah);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
