using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class FightingStyles : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<FightingStyle>();
		public override int PageNum => 2;
	}
}
