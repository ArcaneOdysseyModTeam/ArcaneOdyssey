namespace ArcaneOdyssey.Guidebook.Pages
{
	public class InsanityGuide : GuidebookPage
	{
		public override ushort PageNum => Before<Mutating>();

		public override bool MetConditions(Player player) => player.ArcaneOdyssey().Insanity > 0 || player.ArcaneOdyssey().Banishment > 0;
	}
}
