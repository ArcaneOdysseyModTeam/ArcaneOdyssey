using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.BossRelics;

namespace ArcaneOdyssey.Items.BossRelics
{
	public class EliusBossRelic : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Mystic;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.master = true;
			Item.DefaultToPlaceableTile(ModContent.TileType<EliusBossRelicTile>());
		}
	}
}
