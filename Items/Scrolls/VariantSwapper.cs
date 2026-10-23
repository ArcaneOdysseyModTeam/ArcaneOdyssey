using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Skills.Base;
using System.Linq;

namespace ArcaneOdyssey.Items.Scrolls
{
	public class VariantSwapper : Scroll
	{
		public override ItemRarities Rarity => ItemRarities.Common;

		public override string Texture => AOUtils.GetTexture<EmptyScroll>();

		public override ModSkill Skill => null;

		public override ScrollTier Tier => ScrollTier.Common;

		public override bool CanHaveMagic => true;

		public override bool CanRightClick()
		{
			if (Main.LocalPlayer.PlayerItem().ModItem is MagicType magic)
			{
				return ImbueID.Sets.HasVariants[magic.ID];
			}
			return false;
		}

		public override void RightClick(Player player)
		{
			if (Main.LocalPlayer.PlayerItem().ModItem is MagicType magic && ImbueID.Sets.HasVariants[magic.ID])
			{
				magic.SetMagicVariant(Main.rand.Next(magic.Variants.ToArray()));
			}
		}
	}
}
