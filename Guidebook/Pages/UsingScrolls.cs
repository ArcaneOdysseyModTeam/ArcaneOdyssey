using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class UsingScrolls : GuidebookPage
	{
		public override ushort PageNum => After<ObtainingScrolls>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Scroll>();
	}
}
