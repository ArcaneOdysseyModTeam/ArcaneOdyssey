using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ObtainingScrolls : ModGuidebookPage
	{
		public override int PageNum => After<Imbuing>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
