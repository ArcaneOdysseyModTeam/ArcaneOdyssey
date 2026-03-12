using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items
{
	[LegacyName("Paper")] // common removed items are added here
	public class EmptyScroll : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Junk;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllCommonScrollDrops()));
		}
	}
}
