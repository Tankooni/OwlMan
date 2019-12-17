using Godot;
using System;
using Atmo.OgmoLoader;
using System.Collections.Generic;

public class Overlord : Node
{
	public static float STANDARD_GRAVITY = 18f;

	public static Vector2 LevelBoundsX;
	public static Vector2 LevelBoundsY;
	public static Vector2 ViewportSize;

	public static Node2D Player;

	public Dictionary<string, PackedScene> Sounds = new Dictionary<string, PackedScene>();

	public Node2D Level;

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

		
		Sounds.Add("Squeak", (PackedScene)ResourceLoader.Load("res://prefab/sounds/Squeak.tscn"));
		Sounds.Add("Hit1", (PackedScene)ResourceLoader.Load("res://prefab/sounds/Hit1.tscn"));
		Sounds.Add("Hit3", (PackedScene)ResourceLoader.Load("res://prefab/sounds/Hit3.tscn"));
		Sounds.Add("Hit4", (PackedScene)ResourceLoader.Load("res://prefab/sounds/Hit4.tscn"));
	}

	public void LoadLevel()
	{
		var ogmo = new OgmoLoader();
		Level = ogmo.Load(out Overlord.Player, out Overlord.LevelBoundsX, out Overlord.LevelBoundsY);

		Viewport root = GetTree().GetRoot();
		var CurrentScene = root.GetChild(root.GetChildCount() - 1);

		if (Overlord.Player != null)
			CurrentScene.CallDeferred("add_child", Overlord.Player);
		CurrentScene.CallDeferred("add_child", Level);
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

	public void PlaySound(string name, Vector2 position)
	{
		PackedScene sfxScene;
		if(Sounds.TryGetValue(name, out sfxScene))
		{
			var sfx = (Node2D)sfxScene.Instance();
			Level.AddChild(sfx);
			sfx.SetPosition(position);
		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
