namespace ArcaneOdyssey.Items.Base
{
	public abstract class RareScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Rare;
		public override int AOValue => 1000;
		public override AORarities AORarity => AORarities.Rare;
	}
}
