// using Atmo2.Entities;
// using Utility;

using System;
using Godot;

namespace Atmo2.Movements
{
	public partial class MovementInfo
	{
        public Player PlayerRef;
		public float MoveRefill { get; set; }

		public bool LeftTrace
		{
			get { return PlayerRef.BoxL.Monitoring; }
			set { PlayerRef.BoxL.SetTraceState(value); }
		}
		public bool RightTrace
		{
			get { return PlayerRef.BoxR.Monitoring; }
			set { PlayerRef.BoxR.SetTraceState(value); }
		}
		public bool UpTrace
		{
			get { return PlayerRef.BoxU.Monitoring; }
			set { PlayerRef.BoxU.SetTraceState(value); }
		}
		public bool DownTrace
		{
			get { return PlayerRef.BoxB.Monitoring; }
			set { PlayerRef.BoxB.SetTraceState(value); }
		}

		public void ResetBoxes()
		{
			LeftTrace = false;
			RightTrace = false;
			UpTrace = false;
			DownTrace = false;
		}

		public Vector2 Velocity;
		public Vector2 VelocityMomentum;

		public bool StartShake { get; set; }

		public MovementInfo(Player entity)
		{
			this.PlayerRef = entity;
			Reset();
		}

		public void Reset()
		{
			MoveRefill = 0;
			Velocity.X = 0;
			Velocity.Y = 0;
		}

		Vector2 newVelocity;
		public void Update()
		{
			// Bad implementation of terminal velocity
			newVelocity = new Vector2(Velocity.X, 0);
			if(Mathf.Sign(Velocity.Y) > 0)
			{
				newVelocity.Y = Mathf.Min(Velocity.Y, 2000);
			}
			else
			{
				newVelocity.Y = Velocity.Y;
			}
			PlayerRef.Velocity = newVelocity;

			if(MathF.Abs(newVelocity.X) > PlayerRef.RunSpeed * 1.1f || (PlayerRef.IsOnWall() || !PlayerRef.IsOnFloor()) )
			{
				PlayerRef.FloorSnapLength = 0;
			}
			else
			{
				PlayerRef.FloorSnapLength = 20;
			}

			PlayerRef.MoveAndSlide();
        }
    }
}