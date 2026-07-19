namespace ArcaneOdyssey.Items.Base
{
	public abstract class LostScroll : Scroll
	{
		public sealed override ScrollTier Tier => ScrollTier.Lost;
		public sealed override int Value => 2500;
		public sealed override ItemRarities Rarity => ItemRarities.Mystic;

		public override void Load()
		{
			base.Load();
			ModTypeLookup<LostScroll>.Register(this);
		}
	}
}
