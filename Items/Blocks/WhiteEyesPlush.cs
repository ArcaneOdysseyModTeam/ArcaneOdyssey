using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks
{
	public class WhiteEyesPlush : BaseItem
	{
		public override Rarities Rarity => Rarities.Rare;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<WhiteEyesPlushTile>());
			Item.width = Item.height = 32;
		}
	}
}
