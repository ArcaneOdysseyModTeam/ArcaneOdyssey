using ArcaneOdyssey.Items.Consumable;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Mutating : GuidebookPage
	{
		public override ushort PageNum => After<StrengthWeapons>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<HecateShard>();
	}
}
