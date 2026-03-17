using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ForgingBronze : GuidebookPage
	{
		public override int PageNum => 8;

		public override bool MetConditions(Player player) => NPC.downedBoss2;
	}
}
