using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Skills.Base;

namespace ArcaneOdyssey.Items.Scrolls
{
	public class DevouringScroll : Scroll
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EmptyScroll>();
		}

		public override ItemRarities Rarity => ItemRarities.Common;
		public override string Texture => AOUtils.GetTexture<EmptyScroll>();

		public override ModSkill Skill => null;

		public override ScrollTier Tier => ScrollTier.Common;
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;
		public override bool CanHaveFS => true;

		public override bool CanRightClick()
		{
			if (base.CanRightClick() && Main.LocalPlayer.PlayerItem()?.ModItem is Imbuable imbue)
			{
				for (byte i = 0; i < imbue.Skills.Length; i++)
				{
					if (imbue.Skills[i].Type != imbue.DefaultSkills[i].Type)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void RightClick(Player player)
		{
			if (player.PlayerItem()?.ModItem is Imbuable imbue)
			{
				for (byte i = 0; i < imbue.Skills.Length; i++)
				{
					imbue.RemoveSkill(i);
				}
			}
		}
	}
}
