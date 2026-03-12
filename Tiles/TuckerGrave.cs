using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles
{
	public class TuckerGrave : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);

			AddMapEntry(Color.SaddleBrown, Lang.GetItemName(TileLoader.GetItemDropFromTypeAndStyle(Type)));
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconText = Lang.GetItemName(TileLoader.GetItemDropFromTypeAndStyle(Type)).Value;
			player.cursorItemIconID = -1;
			player.cursorItemIconEnabled = true;
		}
	}
}
