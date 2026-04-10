using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class AboutGodSouls : ModGuidebookPage
	{
		public override int PageNum => After<ForgingBronze>();

		public override bool MetConditions(Player player) => (player.ArcaneOdyssey()?.Souls.Count ?? 1) > 1;
	}
}
