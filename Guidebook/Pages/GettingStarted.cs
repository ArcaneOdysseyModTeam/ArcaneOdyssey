namespace ArcaneOdyssey.Guidebook.Pages
{
	public class GettingStarted : GuidebookPage
	{
		public override bool MetConditions(Player player) => true;

		/// <summary>
		/// always page 0
		/// </summary>
		public override ushort PageNum => 0;
	}
}
