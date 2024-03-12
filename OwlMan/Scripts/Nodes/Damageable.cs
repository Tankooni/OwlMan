using Godot;
using System;
using System.Reflection.Metadata;

[GlobalClass]
public partial class Damageable : Node
{
	public const string DAMAGEABLE_NAME = "Damageable";
	public Action<int> OnDamageCallback;
	public Action OnDeathCallback;
	
	public int Health { get; set; }
	[Export]
	public int MaxHealth = 1;
	public bool InfiniteHealth = false;
	public float InvulnerabilityFrames = 0;
	
	private Node parent;

	public Damageable (Node parent = null, int maxHealth = -1)
	{
		this.parent = parent;
		MaxHealth = maxHealth;
		Name = DAMAGEABLE_NAME;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(MaxHealth < 0)
		{
			InfiniteHealth = true;
		}
		else
		{
			Health = MaxHealth;
		}
	}

	public void HandleDamage(int damage)
	{
		if( InvulnerabilityFrames > 0 )
			return;

		if(OnDamageCallback is not null)
			OnDamageCallback(damage);

		if(InfiniteHealth)
			return;
		Health--;
		if(Health <= 0)
		{
			if(OnDeathCallback is not null)
				OnDeathCallback();
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if( InvulnerabilityFrames > 0 )
			InvulnerabilityFrames--;
    }
}
