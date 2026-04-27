using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class LostScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Lost;
		public override int Value => 2500;
		public override Rarities Rarity => Rarities.Mystic;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LostEmptyScroll>();
		}
	}
}
