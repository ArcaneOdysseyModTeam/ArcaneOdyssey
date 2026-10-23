using ArcaneOdyssey.Walls.Bronze;

namespace ArcaneOdyssey.Walls.UnsafeBronze
{
	public class UnsafeBronzeBrickWall : BronzeBrickWall
	{
		public override string Texture => AOUtils.GetTexture<BronzeBrickWall>();
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.wallHouse[Type] = false;
		}
	}
}
