using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class GettingStarted : ModGuidebookPage
	{
		public override bool MetConditions(Player player) => true;

		/// <summary>
		/// always page 0
		/// </summary>
		public override int PageNum => 0;
	}
}
