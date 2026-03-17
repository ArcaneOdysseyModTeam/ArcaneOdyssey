using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class MagicTypes : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<AOMagic>();
		public override int PageNum => 1;
	}
}
