namespace Atmo2
{
    public class Abilities
	{
        private const float SHITTY_JUMP_STRENTH = 240;
        private const float GOOD_JUMP_STRENTH = 480;


		public bool GoodJump
        {
            get { return player.JumpStrenth == GOOD_JUMP_STRENTH; }
            set
            {
                player.JumpStrenth = value ?
                  GOOD_JUMP_STRENTH :
                  SHITTY_JUMP_STRENTH;
            }
        }
		public bool DoubleJump { get; set; }
		public bool WallSlide { get; set; }
		public bool WallJump { get; set; }
		public bool GroundDash { get; set; }
		public bool AirDash { get; set; }

        private Player player;
		public Abilities(Player player)
		{
            this.player = player;
			GoodJump = false;
			DoubleJump = false;
			WallSlide = false;
			WallJump = false;
			GroundDash = false;
			AirDash = false;
		}

		public void GiveAllAbilities()
		{
			GoodJump = true;
			DoubleJump = true;
			WallSlide = true;
			WallJump = true;
			GroundDash = true;
			AirDash = true;
		}
	}
}
