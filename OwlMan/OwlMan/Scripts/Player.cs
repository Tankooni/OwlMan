using Godot;
using System;
using Atmo2;
using Atmo2.Movements;
using Atmo2.Movements.PlayerStates;
using System.Collections.Generic;
using System.Linq;

public class Player : KinematicBody2D
{
	[Signal]
	public delegate void HealthChanged(int health);
	
	[Signal]
	public delegate void AnimationChanged(String animation);

	public Controller InputController;
	public PlayerStateController PlayerStateController;

	public Abilities Abilities;
	public MovementInfo MovementInfo;
	
	// Player state
	private int health;
	public int Health { 
		get { return health; }
		set {
			health = value;
			EmitSignal(nameof(HealthChanged), health);
		}
	}
	public int Power { get; set; }
	
	private int maxHealth = 5;
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

	public float JumpStrenth { get; set; }
	public float RunSpeed { get; set; }
	public float DashMultiplier { get; set; }
	public float HorizontalDrag { get; set; }
	public float Gravity { get; set; }
	public bool IsInvincable { get; set; }

	public Area2D BoxL;
	public Area2D BoxR;
	public Area2D BoxB;

	// Make this private later and fix the things that reference it to flip the image
	public AnimatedSprite Image;

	private Camera2D _camera;
	private Control _hud;
	private CollisionShape2D _collisionShape2D;
	private Node _overlord;

	public String Animation {
		get { return this.Image.Animation; }
		set { 
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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var node in Atmo.OgmoLoader.OgmoLoader.nodes)
		{
			node.Set("target", GetPath());

			//node.Set("node", node.GetPath());
		}

		_camera = GetNode<Camera2D>("../MainCamera");
		_hud = GetNode<Control>("../CanvasLayer/HUD");
		Image = GetNode<AnimatedSprite>("AnimatedSprite");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_overlord = GetNode("/root/Overlord");

		BoxL = GetNode<Area2D>("SideBoxL");
		BoxR = GetNode<Area2D>("SideBoxR");
		BoxB = GetNode<Area2D>("BottomBox");

		this.Connect("HealthChanged", _hud, "on_set_health");
		this.Connect("AnimationChanged", _hud, "on_animation_changed");

		SetDeferred("Health", maxHealth);
		//Health = maxHealth;
		Power = 0;

		// Spice = 100;
		// Energy = 0f;
		MaxEnergy = 3;
		EnergyRechargeRate = 2f;

		//JumpStrenth = 660;
		RunSpeed = 200;
		DashMultiplier = 3.5f;
		HorizontalDrag = 50;
		Gravity = Overlord.STANDARD_GRAVITY;

		// image.RenderStep = 1;

		// GameWorld.player = this;
		// Type = KQ.CollisionTypePlayer;

		Abilities = new Abilities(this);
		Abilities.GiveAllAbilities();
		MovementInfo = new MovementInfo(this);

		InputController = new Controller();
		PlayerStateController = new PlayerStateController(new PSIdle(this));

		Image.Connect("animation_finished", this, "AnimationComplete");

		// AddResponse(PickupType.AirDash, OnAirDashPickup);
		// AddResponse(PickupType.AirJump, OnAirJumpPickup);
		// AddResponse(PickupType.Jump, OnJumpPickup);
		// AddResponse(PickupType.Dash, OnDashPickup);


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

	// public override bool IsRiding(Solid solid)
	// {
	// 	return Bottom == solid.Top;
	// }

	// public void NodeHandler(System.Xml.XmlNode entity)
	// {
	// }

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		
		InputController.Update();
		// if(Keyboard.Space.Pressed)
		// {
		// 	Console.WriteLine(player_controller.current_state.ToString());
		// }

		PlayerStateController.Update();
		MovementInfo.Update();

		if (InputController.Select())
		{
			_overlord.Call("Reset");
		}
		
		UpdateCamera();

		if (MovementInfo.LeftBox)
		{
			foreach (PhysicsBody2D body in BoxL.GetOverlappingBodies().OfType<PhysicsBody2D>().Where(x => x.IsInGroup("enemy")))
			{
				body.ShapeOwnerSetDisabled(body.ShapeFindOwner(0), true);
				body.QueueFree();
			}
			foreach (var area in BoxL.GetOverlappingAreas().OfType<Area2D>().Where(x => x.IsInGroup("damaging")))
			{
				area.QueueFree();
			}
		}
		if (MovementInfo.RightBox)
		{
			foreach (PhysicsBody2D body in BoxR.GetOverlappingBodies().OfType<PhysicsBody2D>().Where(x => x.IsInGroup("enemy")))
			{
				body.ShapeOwnerSetDisabled(body.ShapeFindOwner(0), true);
				body.QueueFree();
			}
			foreach (var area in BoxR.GetOverlappingAreas().OfType<Area2D>().Where(x => x.IsInGroup("damaging")))
			{
				area.QueueFree();
			}
		}
		if (MovementInfo.BottomBox)
		{
			foreach (PhysicsBody2D body in BoxB.GetOverlappingBodies().OfType<PhysicsBody2D>().Where(x => x.IsInGroup("enemy")))
			{
				body.ShapeOwnerSetDisabled(body.ShapeFindOwner(0), true);
				body.QueueFree();
			}
			foreach (var area in BoxB.GetOverlappingAreas().OfType<Area2D>().Where(x => x.IsInGroup("damaging")))
			{
				area.QueueFree();
			}
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
		ResetPoint.Set(x, y);
	}

	public void ResetPlayerPosition()
	{
		SetPosition(ResetPoint);
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

		if(viewSize == null || viewSize != Overlord.ViewportSize)
		{
			viewSize = Overlord.ViewportSize;
			GD.Print("viewSize: ", viewSize);
		}
		if(levelBoundsX == null || levelBoundsX != Overlord.LevelBoundsX)
		{
			levelBoundsX = Overlord.LevelBoundsX;
			GD.Print("levelBoundsX: ", levelBoundsX);
		}
		if(levelBoundsY == null || levelBoundsY != Overlord.LevelBoundsY)
		{
			levelBoundsY = Overlord.LevelBoundsY;
			GD.Print("levelBoundsY: ", levelBoundsY);
		}

		var centerX = Position.x;
		var centerY = Position.y;

		centerX = Mathf.Clamp(centerX, Overlord.ViewportSize.x / 2f, Overlord.LevelBoundsX.y - Overlord.ViewportSize.x / 2f);
		centerY = Mathf.Clamp(centerY, Overlord.ViewportSize.y / 2f, Overlord.LevelBoundsY.y - Overlord.ViewportSize.y / 2f);

		_camera.SetPosition(new Vector2(centerX, centerY));
	}
	
	public void OnDamage(CollisionObject2D collider)
	{
		if(this.invulnerabilityFrames == 0)
		{ 
			if((collider as CollisionObject2D).IsInGroup("damaging"))
			{
				// Take a damage and do hurt stuff
				Health -= 1;
				invulnerabilityFrames = 120;

				CheckDeath();
			}
		}
	}
	void CheckDeath() {
		if(this.Health <= 0)
		{
			_overlord.Call("Reset");
		}
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
}
