using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Walls.Bronze
{
	public class BronzeBrickWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;

			DustType = DustID.Copper;
			VanillaFallbackOnModDeletion = WallID.GoldBrick;

			AddMapEntry(Color.DarkOrange);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}
