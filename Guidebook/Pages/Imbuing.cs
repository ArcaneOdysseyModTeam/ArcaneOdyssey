using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Imbuing : GuidebookPage
	{
		public override ushort PageNum => After<FightingStyles>();

		public override bool MetConditions(Player player) => player.HasTypeInInventory<Imbuable>();
	}
}
