using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ForgingBronze : GuidebookPage
	{
		public override ushort PageNum => After<SpecialDashes>();

		public override bool MetConditions(Player player) => NPC.downedBoss2;
	}
}
