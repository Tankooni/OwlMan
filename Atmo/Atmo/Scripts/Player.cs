using Godot;
using System;
using Atmo2;
using Atmo2.Movements;
using Atmo2.Movements.PlayerStates;

public class Player : KinematicBody2D
{
	public Controller InputController;
	public PlayerStateController PlayerStateController;

	public Abilities Abilities;
	public MovementInfo MovementInfo;

	private Camera2D camera;
	
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

	public AnimatedSprite image;

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
		camera = GetNode<Camera2D>("../MainCamera");
		image = GetNode<AnimatedSprite>("AnimatedSprite");
		
		// Spice = 100;
		// Energy = 0f;
		MaxEnergy = 3;
		EnergyRechargeRate = 2f;

		//JumpStrenth = 660;
		RunSpeed = 240;
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

		image.Connect("animation_finished", this, "AnimationComplete");

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
		MovementInfo.Update(delta);

		UpdateCamera();

		if (InputController.Select())
		{
			Abilities.GiveAllAbilities();
		}
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

	public void UpdateCamera()
	{
		var centerX = Position.x;
		var centerY = Position.y;

		//TODO: clamping camera to edges of the current room
		// var currentRoom = ((GameWorld)(World)).CurrentRoom;
		// centerX = Mathf.Clamp(centerX, Engine.HalfWidth, currentRoom.RealRoomMeta.width - Engine.HalfWidth);
		// centerY = Mathf.Clamp(centerY, Engine.HalfHeight, currentRoom.RealRoomMeta.height - Engine.HalfHeight);
		
		camera.SetPosition(Position);
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
