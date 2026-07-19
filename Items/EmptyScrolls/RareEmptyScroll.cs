using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.EmptyScrolls
{
	public class RareEmptyScroll : BaseItem
	{
		public override string Texture => AOUtils.GetTexture<EmptyScroll>();

		public override ItemRarities Rarity => ItemRarities.Rare;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllRareScrollDrops()));
		}
	}
}
