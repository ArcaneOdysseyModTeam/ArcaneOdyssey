using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles;

namespace ArcaneOdyssey.Items.Blocks
{
	public class WhiteEyesPlush : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Rare;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<WhiteEyesPlushTile>());
			Item.width = Item.height = 32;
			Item.value = Item.buyPrice(1);
		}
	}
}
