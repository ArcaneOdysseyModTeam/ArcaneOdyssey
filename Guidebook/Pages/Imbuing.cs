using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Imbuing : ModGuidebookPage
	{
		public override int PageNum => After<FightingStyles>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
