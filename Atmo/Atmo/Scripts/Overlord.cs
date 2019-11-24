using Godot;
using System;
using Atmo.OgmoLoader;

public class Overlord : Node
{
	public static float STANDARD_GRAVITY = 18f;

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        var ogmo = new OgmoLoader();
		Node2D player;
		var level = ogmo.Load(out player);

		Viewport root = GetTree().GetRoot();
		var CurrentScene = root.GetChild(root.GetChildCount() - 1);
		CurrentScene.CallDeferred("add_child", level);
		if(player != null)
		{
			CurrentScene.CallDeferred("add_child", player);
		}

		//CurrentScene.AddChild(yeah);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
