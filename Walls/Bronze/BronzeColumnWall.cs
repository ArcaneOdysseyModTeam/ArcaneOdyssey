namespace ArcaneOdyssey.Walls.Bronze
{
	public class BronzeColumnWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Copper;
			VanillaFallbackOnModDeletion = WallID.GoldBrick;

			AddMapEntry(Color.OrangeRed);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}
