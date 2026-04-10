using ArcaneOdyssey.Items.Consumable;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Mutating : ModGuidebookPage
	{
		public override int PageNum => After<StrengthWeapons>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<HecateShard>();
	}
}
