namespace ArcaneOdyssey.Items.Base
{
	public abstract class LostScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Lost;
		public override int AOValue => 2500;
		public override AORarities AORarity => AORarities.Mystic;
	}
}
