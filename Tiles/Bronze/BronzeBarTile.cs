using Terraria.Enums;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.Bronze
{
	public class BronzeBarTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.addTile(Type);
			AddMapEntry(Color.OrangeRed, Language.GetText("MapObject.MetalBar"));
			DustType = DustID.Copper;
		}
	}
}