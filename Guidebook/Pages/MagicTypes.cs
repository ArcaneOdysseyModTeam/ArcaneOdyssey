using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class MagicTypes : ModGuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<MagicType>();
		public override int PageNum => After<GettingStarted>();
	}
}
