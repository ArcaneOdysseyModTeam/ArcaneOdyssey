using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Weapons : GuidebookPage
	{
		public override int PageNum => 7;

		public override bool MetConditions(Player player) => player.HasTypeInInventory<AOWeapon>();
	}
}
