using Godot;
using System;

public partial class HudLayer : CanvasLayer
{
	[Export]
	public TextureRect HealthPips;
	public readonly int pipHeight = 6;
	public readonly int pipWidth = 18;
	[Export]
	public ColorRect HealthBacking;

    public override void _EnterTree()
    {
        base._EnterTree();
		Overlord.HudLayer = this;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	public void Setup(Damageable playerDamageable)
	{
		GD.Print($"player health registered {playerDamageable.Health}");
		int pipBarHeight = playerDamageable.Health * pipHeight;
		HealthPips.SetSize(new Vector2(pipWidth, pipBarHeight));
		HealthBacking.SetSize(new Vector2(pipWidth + 2, pipBarHeight + 2));

		playerDamageable.OnDamageCallback += SetCurrentHealth;
	}

	public void SetCurrentHealth(int damage, Damageable damageable)
	{
		GD.Print($"player health: {damageable.Health}/{damageable.MaxHealth}");
		int pipBarHeight = damageable.Health * pipHeight;
		HealthPips.SetSize(new Vector2(pipWidth, pipBarHeight));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
