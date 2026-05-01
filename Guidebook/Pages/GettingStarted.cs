using ArcaneOdyssey.Items.Consumable;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class GettingStarted : GuidebookPage
	{
		public override bool MetConditions(Player player) => true;

		/// <summary>
		/// always page 0
		/// </summary>
		public override int PageNum => 0;
	}
}
