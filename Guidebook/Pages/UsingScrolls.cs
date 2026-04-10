using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class UsingScrolls : ModGuidebookPage
	{
		public override int PageNum => After<ObtainingScrolls>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Scroll>();
	}
}
