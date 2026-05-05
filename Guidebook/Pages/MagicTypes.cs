using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	internal class MagicTypes : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<MagicType>();
		public override int PageNum => After<GettingStarted>();
	}
}
