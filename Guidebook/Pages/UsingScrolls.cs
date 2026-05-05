using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	internal class UsingScrolls : GuidebookPage
	{
		public override int PageNum => After<ObtainingScrolls>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Scroll>();
	}
}
