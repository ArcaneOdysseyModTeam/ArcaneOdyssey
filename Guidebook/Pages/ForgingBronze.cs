using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	internal class ForgingBronze : GuidebookPage
	{
		public override int PageNum => After<UsingScrolls>();

		public override bool MetConditions(Player player) => NPC.downedBoss2;
	}
}
