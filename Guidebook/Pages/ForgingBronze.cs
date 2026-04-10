using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ForgingBronze : ModGuidebookPage
	{
		public override int PageNum => After<UsingScrolls>();

		public override bool MetConditions(Player player) => NPC.downedBoss2;
	}
}
