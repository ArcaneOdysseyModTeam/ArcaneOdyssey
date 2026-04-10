using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class RareScroll : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Rare;
		public override int Value => 1000;
		public override Rarities Rarity => Rarities.Rare;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EmptyScroll>();
		}
	}
}
