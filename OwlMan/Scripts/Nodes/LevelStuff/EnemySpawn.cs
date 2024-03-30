using Atmo2.Enemy;
using Godot;
using System;

[GlobalClass]
[Tool]
public partial class EnemySpawn : Node2D
{	
	[Export]
	public PackedScene EnemyToSpawn;

	private Enemy ownedEnemy;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

#if TOOLS
		
#endif
		if ( EnemyToSpawn != null )
		{
			var newEntity = EnemyToSpawn.Instantiate() as Enemy;
			if( newEntity != null )
			{
				newEntity.Position = GlobalPosition;
				GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, newEntity);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Draw()
    {
        base._Draw();
		// DrawCircle(Vector2.Zero, 100, Colors.White);
		if( ownedEnemy != null )
		{
			// ownedEnemy.Sprite2D.Animation
			// DrawTexture(ownedEnemy.Sprite2D.SpriteFrames.GetFrameTexture(
			// 	ownedEnemy.Sprite2D.Animation, ownedEnemy.Sprite2D.Frame), Vector2.Zero);
		}
#if TOOLS
		
#endif
		// DrawTexture()
    }
}
