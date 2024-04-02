using Godot;
using System;
using Atmo2;
using Atmo2.Movements;
using Atmo2.Movements.PlayerStates;
using Atmo2.Enemy;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HealthChangedEventHandler(int health);

	[Signal]
	public delegate void AnimationChangedEventHandler(String animation);

	public Controller InputController;
	public PlayerStateController PlayerStateController;

	public Abilities Abilities;
	public MovementInfo MovementInfo;

	public InteractComponent Interactable;
	public String IDLabel;

	// Player state
	public int Power { get; set; }

	private int maxHealth = 30;
	private int maxPower;

	private int invulnerabilityFrames = 0;

	private int spice;
	public int Spice
	{
		get { return spice; }
		set
		{
			spice = Mathf.Clamp(value, 0, 100);
			if (spice == 0)
				PlayerStateController.NextState(
					new PSDeath(this));
		}
	}
	private float energy;
	public float Energy
	{
		get { return energy; }
		set
		{
			energy = Mathf.Clamp(value, 0, MaxEnergy);
		}
	}
	public int MaxEnergy { get; set; }
	public float EnergyRechargeRate { get; set; }

	private float facingDirection = 1;
	public float FacingDirection
	{
		get { return facingDirection; }
		set
		{
			if( value != 0 )
				Image.FlipH = (facingDirection = value) < 0;
			else
				Image.FlipH = (facingDirection = 1) < 0;
		}
	}

	public float JumpStrenth { get; set; }
	public float RunSpeed { get; set; }
	public float DashMultiplier { get; set; }
	public float HorizontalAirDrag { get; set; }
	public float HorizontalGroundDrag { get; set; }
	public float Gravity { get; set; }
	public bool IsInvincable { get; set; }

	[Export]
	public KillBox BoxL;
	[Export]
	public KillBox BoxR;
	[Export]
	public KillBox BoxU;
	[Export]
	public KillBox BoxB;

	[Export]
	public AnimatedSprite2D Image;
	[Export]
	private CollisionShape2D collisionShape2D;
	[Export]
	public Damageable Damageable;
	

	public Godot.Collections.Array AllInteractions = new Godot.Collections.Array();

	private ShakeCamera camera;
	private Overlord _overlord;

	public Vector2 HitBounceDirection = new();

	public String Animation
	{
		get { return this.Image.Animation; }
		set
		{
			this.Image.Play(value);
			this.EmitSignal("AnimationChanged", value);
		}
	}

	// public float Alpha
	// {
	//	 get { return image.Alpha; }
	//	 set { image.Alpha = value; }
	// }

	// private Orb jumpOrb;
	// private Orb dashOrb;
	// private List<Orb> orbs = new List<Orb>();

	// Make this a property at some point
	public Vector2 ResetPoint;

    public override void _EnterTree()
    {
        base._EnterTree();
		Enemy.PlayerPath = this.GetPath();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
		
		_overlord = GetNode<Overlord>("/root/Overlord");

		AddToGroup(HitGroups.Player);

		camera = Overlord.MainCamera;
		camera.SetFollow(this);
		// camera?.Call("SetFollow", this.GetPath());

		//foreach (var node in Atmo.OgmoLoader.OgmoLoader.nodes)
		//{
		//	node.Set("target", GetPath());
		//	Enemy.PlayerPath = GetPath();

		//	//node.Set("node", node.GetPath());
		//}

		BoxL.HitCallback += OnTraceHit;
		BoxR.HitCallback += OnTraceHit;
		BoxU.HitCallback += OnTraceHit;
		BoxB.HitCallback += OnTraceHitDown;

		Damageable.MaxHealth = maxHealth;
		Damageable.Health = maxHealth;

		Damageable.OnDamageCallback += HandleDamage;
		// damageable.OnDeathCallback += HandleDeath;
		Overlord.HudLayer.CallDeferred(HudLayer.MethodName.Setup, Damageable);
		
		//Health = maxHealth;

		Power = 0;

		// Spice = 100;
		// Energy = 0f;
		MaxEnergy = 1;
		EnergyRechargeRate = 2f;

		//JumpStrenth = 660;
		RunSpeed = 200;
		DashMultiplier = 3.5f;
		HorizontalAirDrag = 30;
		HorizontalGroundDrag = 50;
		Gravity = Overlord.STANDARD_GRAVITY;

		// image.RenderStep = 1;

		// GameWorld.player = this;
		// Type = KQ.CollisionTypePlayer;

		Abilities = new Abilities(this);
		// Abilities.GiveAllAbilities();
		MovementInfo = new MovementInfo(this);

		InputController = new Controller();
		PlayerStateController = new PlayerStateController(new PSIdle(this));

		Image.Connect("animation_finished", new Callable(this, "AnimationComplete"));


		// AddResponse(PickupType.AirDash, OnAirDashPickup);
		// AddResponse(PickupType.AirJump, OnAirJumpPickup);
		// AddResponse(PickupType.Jump, OnJumpPickup);
		// AddResponse(PickupType.Dash, OnDashPickup);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

	public void AnimationComplete()
	{
		PlayerStateController.AnimationComplete();
	}

	public void RefillEnergy()
	{
		Energy = MaxEnergy;/*MathHelper.Clamp(
			time.Elapsed*EnergyRechargeRate + Energy, 0, MaxEnergy);*/
	}

	public void OnTraceHitDown(Node2D otherNode2D)
	{
		HitBounceDirection = new Vector2(0, -1);
		OnTraceHit(otherNode2D);
	}

	public void OnTraceHit(Node2D otherNode2D)
	{
		ShakeCamera();
	}

	public void ShakeCamera()
	{
		camera.Call("Shake", .1f, 100, 10);
		Overlord.OwlOverlord.PlaySound("Hit4", Position);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		InputController.Update();

		PlayerStateController.Update();
		MovementInfo.Update();

		if (InputController.Select())
		{
			_overlord.Reset();
		}

		invulnerabilityFrames = Math.Max(--invulnerabilityFrames, 0);
	}

	// public override void Squish()
	// {
	// 	//World.Remove(this);
	// 	this.ResetPlayerPosition();
	// }

	public void SetResetPointToCurrentLocation()
	{
		SetResetPoint(Position);
	}

	public void SetResetPoint(Vector2 position)
	{
		ResetPoint = position;
	}
	public void SetResetPoint(float x, float y)
	{
		ResetPoint = new Vector2(x, y);
	}

	public void ResetPlayerPosition()
	{
		this.Position = ResetPoint;
		UpdateCamera();
	}

	Vector2 viewSize;
	Vector2 levelBoundsX;
	Vector2 levelBoundsY;

	public void UpdateCamera()
	{
		//var centerX = Position.x;
		//var centerY = Position.y;

		//TODO: clamping camera to edges of the current room
		// var currentRoom = ((GameWorld)(World)).CurrentRoom;
		// centerX = Mathf.Clamp(centerX, Engine.HalfWidth, currentRoom.RealRoomMeta.width - Engine.HalfWidth);
		// centerY = Mathf.Clamp(centerY, Engine.HalfHeight, currentRoom.RealRoomMeta.height - Engine.HalfHeight);

		// if(MovementInfo.StartShake)
		// {
		// 	_camera.Call("Shake", 10f, .1f, 10);
		// 	MovementInfo.StartShake = false;
		// }


	}

	public void HandleDamage(int damage, Damageable myDamageable)
	{
		if (myDamageable.InvulnerabilityFrames == 0)
		{
			myDamageable.InvulnerabilityFrames = 120;
		}

		EmitSignal(nameof(HealthChanged), myDamageable.Health);
	}
	public void HandleDeath()
	{
		_overlord.Reset();
	}

	// public void OnJumpPickup(object[] param)
	// {
	// 	Abilities.GoodJump = true;
	// 	if (jumpOrb == null)
	// 	{
	// 		jumpOrb = new Orb(X, Y, OrbType.Yellow, orbs.Any() ? (Entity)orbs.First() : (Entity)this);
	// 		orbs.Add(World.Add(jumpOrb));
	// 	}
	// }
	// public void OnAirJumpPickup(object[] param)
	// {
	// 	Abilities.DoubleJump = true;
	// 	if(MaxEnergy < 4)
	// 		MaxEnergy++;
	// 	if (jumpOrb == null)
	// 	{
	// 		jumpOrb = new Orb(X, Y, OrbType.Yellow, orbs.Any() ? (Entity)orbs.Last() : (Entity)this);
	// 		orbs.Add(World.Add(jumpOrb));
	// 	}
	// 	else
	// 		jumpOrb.IsActivated = true;
	// }
	// public void OnDashPickup(object[] param)
	// {
	// 	Abilities.GroundDash = true;
	// 	if (dashOrb == null)
	// 	{
	// 		dashOrb = new Orb(X, Y, OrbType.Blue, orbs.Any() ? (Entity)orbs.Last() : (Entity)this);

	// 		orbs.Add(World.Add(dashOrb));
	// 	}
	// }
	// public void OnAirDashPickup(object[] param)
	// {
	// 	Abilities.AirDash = true;
	// 	if (MaxEnergy < 4)
	// 		MaxEnergy++;
	// 	if (dashOrb == null)
	// 	{
	// 		dashOrb = new Orb(X, Y, OrbType.Blue, orbs.Any() ? (Entity)orbs.Last() : (Entity)this);
	// 		orbs.Add(World.Add(dashOrb));
	// 	}
	// 	else
	// 		dashOrb.IsActivated = true;
	// }

	//// Interaction Methods ////
	
	public void OnAreaEntered(Area2D InteractionArea)
	{
		AllInteractions.Insert(0, InteractionArea);
	}

	public void OnAreaExited(Area2D InteractionArea)
	{
		AllInteractions.Remove(InteractionArea);
	}
	
	public bool HasInteract()
	{
		if (Interactable is not null)
		{
			GD.Print("Player has an interact.");
			GD.Print(Interactable);
			return true;
		}
		else 
		{
			return false;
		}
	}
}
