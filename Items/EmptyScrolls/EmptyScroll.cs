using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.EmptyScrolls
{
	[LegacyName("Paper")]
	public class EmptyScroll : BaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllCommonScrollDrops()));
		}
	}
}
