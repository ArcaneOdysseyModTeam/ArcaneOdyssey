using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ImbueStacking : GuidebookPage
	{
		public override int PageNum => Before<AboutGodSouls>();

		public override bool MetConditions(Player player) => Main.hardMode;
	}
}
