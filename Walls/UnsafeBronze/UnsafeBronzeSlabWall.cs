using ArcaneOdyssey.Walls.Bronze;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Walls.UnsafeBronze
{
	public class UnsafeBronzeSlabWall : ModWall
	{
		public override string Texture => AOUtils.GetTexture<BronzeSlabWall>();
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;

			DustType = DustID.Copper;
			VanillaFallbackOnModDeletion = WallID.GoldBrick;

			AddMapEntry(Color.OrangeRed);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override bool CanExplode(int i, int j) => false;

		public override bool Drop(int i, int j, ref int type) => false;
	}
}
