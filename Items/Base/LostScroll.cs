using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class LostScroll : Scroll
	{
		public sealed override ScrollTier Tier => ScrollTier.Lost;
		public sealed override int Value => 2500;
		public sealed override ItemRarities Rarity => ItemRarities.Mystic;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LostEmptyScroll>();
		}
		public override void Load()
		{
			base.Load();
			ModTypeLookup<LostScroll>.Register(this);
		}
	}
}
