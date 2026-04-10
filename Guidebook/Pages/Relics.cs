using ArcaneOdyssey.Imbues.Relics;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Relics : ModGuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<SpiritEnergy>();
		public override int PageNum => After<FightingStyles>();
	}
}
