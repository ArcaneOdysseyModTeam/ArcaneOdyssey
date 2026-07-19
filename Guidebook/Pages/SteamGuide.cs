using ArcaneOdyssey.Imbues;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class SteamGuide : GuidebookPage
	{
		public override ushort PageNum => Before<ForgingBronze>();

		public override bool MetConditions(Player player) => player.HasItemInInventory(e => e.Imbue() is SteamImbue || e.SecondImbue() is SteamImbue);
	}
}
