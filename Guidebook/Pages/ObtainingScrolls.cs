using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ObtainingScrolls : GuidebookPage
	{
		public override ushort PageNum => After<Imbuing>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
