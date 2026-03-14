using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class RareScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Rare;
		public override int AOValue => 1000;
		public override AORarities AORarity => AORarities.Rare;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CommonEmptyScroll>();
		}
	}
}
