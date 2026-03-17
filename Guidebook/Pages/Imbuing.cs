using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Imbuing : GuidebookPage
	{
		public override int PageNum => 4;

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
