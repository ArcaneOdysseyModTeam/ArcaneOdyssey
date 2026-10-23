namespace ArcaneOdyssey.Tiles.Bronze
{
	public class BronzeSlabTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tileBlockLight[Type] = true;

			DustType = DustID.Copper;
			VanillaFallbackOnModDeletion = TileID.GoldBrick;

			AddMapEntry(Color.DarkOrange);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}