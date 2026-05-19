using ArcaneOdyssey.Items.Consumable;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Mutating : GuidebookPage
	{
		public override ushort PageNum => After<StrengthWeapons>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<HecateShard>();
	}
}
