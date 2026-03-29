using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.BossRelics;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.BossRelics
{
	public class EliusBossRelic : BaseItem
	{
		public override Rarities Rarity => Rarities.Mystic;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.master = true;
			Item.DefaultToPlaceableTile(ModContent.TileType<EliusBossRelicTile>());
		}
	}
}
