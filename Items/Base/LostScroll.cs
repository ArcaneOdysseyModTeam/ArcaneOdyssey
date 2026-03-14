using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class LostScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Lost;
		public override int AOValue => 2500;
		public override AORarities AORarity => AORarities.Mystic;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<RareEmptyScroll>();
		}
	}
}
