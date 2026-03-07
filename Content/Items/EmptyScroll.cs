using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.GlobalTypes;
using Terraria;

namespace ArcaneOdyssey.Content.Items
{
	public class EmptyScroll : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Unknown;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllCommonScrollDrops()));
		}
	}
}
