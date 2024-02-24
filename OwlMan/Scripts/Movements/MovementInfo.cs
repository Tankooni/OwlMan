// using Atmo2.Entities;
// using Utility;

using Godot;

namespace Atmo2.Movements
{
	public partial class MovementInfo
	{
        private Player entity;
		public bool OnGround { get; set; }
		public bool HeadBonk { get; set; }
		public bool AgainstLeftWall { get; set; }
		public bool AgainstRightWall { get; set; }
		public float MoveRefill { get; set; }

		public bool LeftBox { get; set; }
		public bool RightBox { get; set; }
		public bool BottomBox { get; set; }

		public void ResetBoxes()
		{
			LeftBox = false;
			RightBox = false;
			BottomBox = false;
		}

		public Vector2 Velocity;
		public Vector2 VelocityMomentum;

		public bool StartShake { get; set; }

		public MovementInfo(Player entity)
		{
			this.entity = entity;
			Reset();
		}

		public void Reset()
		{
			OnGround = false;
			HeadBonk = false;
			AgainstLeftWall = false;
			AgainstRightWall = false;
			MoveRefill = 0;
			Velocity.X = 0;
			Velocity.Y = 0;
		}

		public void Update()
		{
			entity.Velocity = Velocity;
			entity.MoveAndSlide();

			OnGround = entity.TestMove(entity.Transform, new Vector2(0, 1));
			HeadBonk = entity.TestMove(entity.Transform, new Vector2(0, -1));
			AgainstLeftWall = entity.TestMove(entity.Transform, new Vector2(-1, 0));
			AgainstRightWall = entity.TestMove(entity.Transform, new Vector2(1, 0));




   //         this.OnGround = entity.Collide(KQ.CollisionTypeSolid, entity.X, entity.Y + 1) != null;
			//foreach (var node in Nodes.AllNodes)
			//{
			//	Entity jjj = entity.World.CollideLine(KQ.CollisionTypePlayer, node.BottomLeft.X, node.BottomLeft.Y - 1, node.TopRight.X, node.TopRight.Y - 1);
			//	if (jjj == entity)
			//	{
			//		this.OnGround = true;
			//		break;
			//		//this.OnGround =
			//	}
			//}
			/*this.AgainstWall += (entity.Collide(KQ.CollisionTypeSolid, entity.X + 1, entity.Y) != null) ? 1 : 0;
            this.AgainstWall -= (entity.Collide(KQ.CollisionTypeSolid, entity.X - 1, entity.Y) != null) ? 1 : 0;*/
        }
    }
}