using ArcaneOdyssey.Walls.Bronze;

namespace ArcaneOdyssey.Walls.UnsafeBronze
{
	public class UnsafeBronzeSlabWall : BronzeSlabWall
	{
		public override string Texture => AOUtils.GetTexture<BronzeSlabWall>();
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.wallHouse[Type] = false;
		}
	}
}
