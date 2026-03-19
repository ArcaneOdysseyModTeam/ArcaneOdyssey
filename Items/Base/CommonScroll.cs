using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class CommonScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Common;
		public override int AOValue => 100;
		public override AORarities AORarity => AORarities.Uncommon;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EmptyScroll>();
		}
	}
}
