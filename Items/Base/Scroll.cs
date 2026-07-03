using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Skills.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class Scroll : BaseItem
	{
		public abstract ModSkill Skill { get; }

		public override void Load()
		{
			ModTypeLookup<Scroll>.Register(this);
		}

		public bool HasCorrectImbue = false;
		public Imbuable Imbue = null;
		public Imbuable SecondImbue = null;

		public abstract ScrollTier Tier { get; }

		public virtual bool MetConditions() => true;

		public override void RightClick(Player player)
		{
			if (Main.LocalPlayer.PlayerItem().ModItem is Imbuable imbue && CanBeAppliedTo(imbue))
			{
				switch (Skill.SkillSlot)
				{
					case ModSkill.SkillType.Attack:
						imbue.SetSkill(imbue.selectedIndex, Skill);
						break;
					case ModSkill.SkillType.Passive:
						imbue.SetSkill(3, Skill);
						break;
					case ModSkill.SkillType.Mobility:
						imbue.SetSkill(4, Skill);
						break;
					case ModSkill.SkillType.Dash:
						imbue.SetSkill(5, Skill);
						break;
				}
			}
		}

		public override bool CanRightClick() => Main.LocalPlayer.PlayerItem().ModItem is Imbuable imbue && CanBeAppliedTo(imbue);

		public bool CanBeAppliedTo(Imbuable imbue) => (CanHaveMagic && imbue is MagicType) || (CanHaveRelic && imbue is SpiritEnergy) || (CanHaveFS && imbue is FightingStyle);

		public virtual bool CanHaveMagic => false;
		public virtual bool CanHaveRelic => false;
		public virtual bool CanHaveFS => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
		}

		public string TierFormatting
		{
			get
			{
				var text = "";
				if (CanHaveFS)
				{
					text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Technique");
				}
				if (CanHaveMagic)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "|";
					}
					text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Spell");
				}
				if (CanHaveRelic)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "|";
					}
					text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Rite");
				}
				return text;
			}
		}

		public string ReqFormatting
		{
			get
			{
				var text = "";
				if (CanHaveFS)
				{
					text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.FightingStyle");
				}
				if (CanHaveMagic)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "|";
					}
					text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Magic");
				}
				if (CanHaveRelic)
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += "|";
					}
					text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Relic");
				}
				return text;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.AddTooltip(new(Mod, "ScrollTier", ArcaneOdysseyMod.Instance.CustomLocalization($"ScrollTiers.{Tier}", TierFormatting).Value));
			tooltips.AddTooltip(new(Mod, "ScrollReq", ArcaneOdysseyMod.Instance.CustomLocalization($"ScrollTiers.NeedsImbue", ReqFormatting).Value));
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EmptyScroll>();
			ArcaneOdysseyMod.Sets.showItemTypeTooltip[Type] = false;
		}
	}
}
