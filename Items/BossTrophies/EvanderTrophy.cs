using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.BossTrophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.BossTrophies
{
	public class EvanderTrophy : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<EvanderTrophyTile>());
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 1);
			Item.width = Item.height = 32;
		}
	}
}
