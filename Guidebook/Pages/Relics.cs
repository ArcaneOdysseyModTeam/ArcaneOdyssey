using ArcaneOdyssey.Imbues.Relics;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class Relics : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<SpiritEnergy>();
		public override ushort PageNum => After<FightingStyles>();
	}
}
