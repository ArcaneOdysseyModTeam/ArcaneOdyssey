using ArcaneOdyssey.Content.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Items
{
	public class EmptyScroll : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Unknown;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(TileLoot.GetAllCommonScrollDrops()));
		}
	}
}
