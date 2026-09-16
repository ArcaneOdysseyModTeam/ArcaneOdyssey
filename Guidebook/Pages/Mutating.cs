using ArcaneOdyssey.Items.Consumable;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Mutating : GuidebookPage
	{
		public override ushort PageNum => After<SavantWeapons>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<HecateShard>();
	}
}
