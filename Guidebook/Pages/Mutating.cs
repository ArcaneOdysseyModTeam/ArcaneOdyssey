using ArcaneOdyssey.Items.Consumable;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Mutating : GuidebookPage
	{
		public override int PageNum => 11;

		public override bool MetConditions(Player player) => player.HasTypeInInventory<HecateShard>();
	}
}
