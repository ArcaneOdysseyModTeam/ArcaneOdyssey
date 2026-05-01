using ArcaneOdyssey.Imbues;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class SteamGuide : GuidebookPage
	{
		public override int PageNum => Before<ForgingBronze>();

		public override bool MetConditions(Player player) => player.HasItemInInventory(e => e.Imbue() is SteamImbue || e.SecondImbue() is SteamImbue);
	}
}
