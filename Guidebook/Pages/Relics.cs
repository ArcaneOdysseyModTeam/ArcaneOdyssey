using ArcaneOdyssey.Imbues.Relics;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	internal class Relics : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<SpiritEnergy>();
		public override int PageNum => After<FightingStyles>();
	}
}
