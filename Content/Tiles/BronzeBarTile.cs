using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Content.Tiles
{
	public class BronzeBarTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			RegisterItemDrop(ModContent.ItemType<BronzeBar>());
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addTile(Type);
			AddMapEntry(Color.DarkOrange, Language.GetText("MapObject.MetalBar"));
			DustType = DustID.Copper;
		}
	}
}