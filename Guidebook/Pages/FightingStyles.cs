using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class FightingStyles : ModGuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<FightingStyle>();
		public override int PageNum => After<MagicTypes>();
	}
}
