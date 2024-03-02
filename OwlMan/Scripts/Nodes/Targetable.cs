using Godot;
using System;

public partial class Targetable : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Name = nameof(Targetable);
	}
}
