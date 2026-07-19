using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.BossTrophies;

namespace ArcaneOdyssey.Items.BossTrophies
{
	public class EliusTrophy : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<EliusTrophyTile>());
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 1);
			Item.width = Item.height = 32;
		}
	}
}
