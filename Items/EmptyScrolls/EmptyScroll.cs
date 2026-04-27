using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.EmptyScrolls
{
	[LegacyName("TitleMusicBox", "Paper")]
	public class EmptyScroll : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllCommonScrollDrops()));
		}
	}
}
