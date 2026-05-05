using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	internal class ObtainingScrolls : GuidebookPage
	{
		public override int PageNum => After<Imbuing>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
