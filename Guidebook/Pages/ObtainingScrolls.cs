using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ObtainingScrolls : GuidebookPage
	{
		public override int PageNum => 5;

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
