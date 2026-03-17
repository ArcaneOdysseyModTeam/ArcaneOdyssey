using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class AboutGodSouls : GuidebookPage
	{
		public override int PageNum => 9;

		public override bool MetConditions(Player player) => (player.ArcaneOdyssey()?.Souls.Count ?? 1) > 1;
	}
}
