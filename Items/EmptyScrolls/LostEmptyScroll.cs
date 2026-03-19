using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Items.EmptyScrolls
{
	public class LostEmptyScroll : AOBaseItem
	{
		public override string Texture => AOUtils.GetTexture<EmptyScroll>();

		public override AORarities AORarity => AORarities.Mystic;

		public override void UpdateInventory(Player player)
		{
			Item.SetDefaults(Main.rand.Next(AOTile.GetAllLostScrollDrops()));
		}
	}
}
