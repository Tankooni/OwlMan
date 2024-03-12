using Godot;
using System;

[GlobalClass]
[Tool]
public partial class PlayerStart : Node2D
{
	[Export]
	public PackedScene SpawnThis;

    public override void _EnterTree()
    {
        base._EnterTree();
		
		if( !Engine.IsEditorHint() )
		{
			if ( SpawnThis != null )
			{
				var newPlayer = SpawnThis.Instantiate<Player>();
				if( newPlayer != null )
				{
					newPlayer.Position = GlobalPosition;
					GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, newPlayer);
				}
					
			}
			// ResourceLoader.
			// var tempPlayer = SpawnThis.inst
			// AddChild(tempPlayer);
			// tempPlayer.Owner = GetTree().CurrentScene;
		}
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		if( Engine.IsEditorHint() )
		{
			// var tempPlayer = new Player();
			// AddChild(tempPlayer.Image);
			// GD.Print("Player Start Ready");
			// tempPlayer.Owner = 
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
