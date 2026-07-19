namespace ArcaneOdyssey.Items.Base
{
	public abstract class CommonScroll : Scroll
	{
		public sealed override ScrollTier Tier => ScrollTier.Common;
		public sealed override int Value => 100;
		public sealed override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void Load()
		{
			base.Load();
			ModTypeLookup<CommonScroll>.Register(this);
		}
	}
}
