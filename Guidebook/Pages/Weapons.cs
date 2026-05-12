using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Weapons : GuidebookPage
	{
		public override int PageNum => After<ForgingBronze>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Weapon>(out var weap) && weap.Ability.HasValue;
	}
}
