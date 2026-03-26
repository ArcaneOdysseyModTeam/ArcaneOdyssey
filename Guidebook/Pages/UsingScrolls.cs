using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class UsingScrolls : GuidebookPage
	{
		public override int PageNum => After<ObtainingScrolls>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Scroll>();
	}
}
