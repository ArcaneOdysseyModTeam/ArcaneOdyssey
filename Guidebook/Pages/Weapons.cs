using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Weapons : ModGuidebookPage
	{
		public override int PageNum => After<UsingScrolls>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Weapon>();
	}
}
