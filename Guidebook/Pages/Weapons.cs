using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Weapons : GuidebookPage
	{
		public override ushort PageNum => After<ForgingBronze>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Weapon>(out var weap) && weap.Ability.HasValue;
	}
}
