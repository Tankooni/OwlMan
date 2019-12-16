using Godot;
using System;
using Atmo.OgmoLoader;

public class Overlord : Node
{
	public static float STANDARD_GRAVITY = 18f;

	public static Vector2 LevelBoundsX;
	public static Vector2 LevelBoundsY;
	public static Vector2 ViewportSize;

	public static Node2D Player;

	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		Overlord.ViewportSize = new Vector2
		(
			x: (int)ProjectSettings.GetSetting("display/window/size/width"),
			y: (int)ProjectSettings.GetSetting("display/window/size/height")
		);
		LoadLevel();
	}

	public void LoadLevel()
	{
		var ogmo = new OgmoLoader();
		var level = ogmo.Load(out Overlord.Player, out Overlord.LevelBoundsX, out Overlord.LevelBoundsY);

		Viewport root = GetTree().GetRoot();
		var CurrentScene = root.GetChild(root.GetChildCount() - 1);

		if (Overlord.Player != null)
			CurrentScene.CallDeferred("add_child", Overlord.Player);
		CurrentScene.CallDeferred("add_child", level);
	}

	public void Reset()
	{
		//Remove the current level
		var level = GetNode("../GameScene");
		level.GetParent().RemoveChild(level);
		level.CallDeferred("free");

		//Add the next level
		var nextLevel = ((PackedScene)ResourceLoader.Load("res://scenes/GameScene.tscn")).Instance();
		GetNode("/root").AddChild(nextLevel);

		LoadLevel();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
