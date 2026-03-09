using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.GlobalTypes;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items
{
	[LegacyName("Paper")] // common removed items are added here
	public class EmptyScroll : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Unknown;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllCommonScrollDrops()));
		}
	}
}
