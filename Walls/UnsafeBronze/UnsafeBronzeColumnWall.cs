using ArcaneOdyssey.Walls.Bronze;

namespace ArcaneOdyssey.Walls.UnsafeBronze
{
	public class UnsafeBronzeColumnWall : BronzeColumnWall
	{
		public override string Texture => AOUtils.GetTexture<BronzeColumnWall>();
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.wallHouse[Type] = false;
		}
	}
}
