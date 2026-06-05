using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class RareScroll : Scroll
	{
		public sealed override ScrollTier Tier => ScrollTier.Rare;
		public sealed override int Value => 1000;
		public sealed override ItemRarities Rarity => ItemRarities.Rare;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<RareEmptyScroll>();
		}
	}
}
