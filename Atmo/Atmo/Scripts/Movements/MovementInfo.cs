// using Atmo2.Entities;
// using Utility;

using Godot;

namespace Atmo2.Movements
{
	public class MovementInfo
	{
        private Player entity;
		public bool OnGround { get; set; }
		public bool HeadBonk { get; set; }
		public int AgainstWall { get; set; }
		public float MoveRefill { get; set; }

		public float VelX { get; set; }
		public float VelY { get; set; }

		public MovementInfo(Player entity)
		{
			this.entity = entity;
			MoveRefill = 0;
			VelX = 0;
			VelY = 0;

			Reset();
		}

		public void Reset()
		{
			OnGround = false;
			HeadBonk = false;
			AgainstWall = 0;
			VelX = 0;
			VelY = 0;
		}

		public void Update(float delta)
		{
			entity.MoveAndSlide(new Vector2(VelX, VelY));

			OnGround = entity.TestMove(entity.Transform, new Vector2(0, 1));
			HeadBonk = entity.TestMove(entity.Transform, new Vector2(0, -1));
			AgainstWall = entity.TestMove(entity.Transform, new Vector2(1, 0)) ? 1 : 0;
			AgainstWall -= entity.TestMove(entity.Transform, new Vector2(-1, 0)) ? 1 : 0;


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