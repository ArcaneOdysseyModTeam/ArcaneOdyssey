using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class Scroll : BaseItem, IImbuable
	{
		public override void Load()
		{
			ModTypeLookup<Scroll>.Register(this);
		}

		public void ActivateAbility(Player player)
		{
			if (Ability.HasValue)
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, Ability.Value.Colour, ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Exclaim", Ability.Value.Name).Value.Trim(), true);
				}
			}
		}

		public virtual bool MetConditions() => true;

		public LocalizedText SkillName => Language.GetOrRegister(this.GetLocalizationKey("SkillName"), PrettyPrintName);

		public WeaponAbility? Ability
		{
			get
			{
				if (HasCorrectImbue)
				{
					var ab = new WeaponAbility(SkillName.Value, null, Imbue.Colour);
					if (Imbue is not FightingStyle)
					{
						ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", Imbue.PrettySpellPrefix, ab.Name).Value.Trim();
					}
					else if (SecondImbue is not null)
					{
						ab.Colour = SecondImbue.Colour;
					}
					if (SecondImbue is not null)
					{
						ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", SecondImbue.PrettyAttackPrefix, ab.Name).Value.Trim();
					}
					return ab;
				}
				return null;
			}
		}

		public float ApplySpeed(float value, bool flipfloat = false)
		{
			if (BenifitsFromScrollStats.HasValue)
			{
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public bool? BenifitsFromScrollStats => Item.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public abstract ScrollTier Tier { get; }

		public Imbuable Imbue
		{
			get
			{
				return Item?.ArcaneOdyssey()?.Imbue;
			}
			set
			{
				if (Item?.ArcaneOdyssey() is not null)
				{
					Item.ArcaneOdyssey().Imbue = value;
				}
			}
		}

		public Imbuable SecondImbue
		{
			get
			{
				return Item?.ArcaneOdyssey()?.SecondImbue;
			}
			set
			{
				if (Item?.ArcaneOdyssey() is not null)
				{
					Item.ArcaneOdyssey().SecondImbue = value;
				}
			}
		}

		public virtual bool CanHaveMagic => false;
		public virtual bool CanHaveRelic => false;
		public virtual bool CanHaveFS => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Rapier;
		}

		public override void UpdateInventory(Player player)
		{
			if (HasCorrectImbue)
			{
				Item.color = Color.Lerp(Color.Transparent, Imbue.Colour, .75f);
			}
			else Item.color = Color.Transparent;
		}

		public override void UpdateEquip(Player player)
		{
			if (Item.CanHaveImbue(player.Imbue()))
			{
				Imbue = player.Imbue();
			}
			else
			{
				Imbue = null;
			}

			if (this is not AuraScroll)
			{
				SecondImbue = Imbue?.Imbue;
			}

			if (HasCorrectImbue)
			{
				Item.color = Color.Lerp(Color.Transparent, Imbue.Colour, .75f);
			}
			else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player) => Imbue is not null && !Item.accessory;

		public string GetTierFormatting()
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
					text += "/";
				}
				text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Spell");
			}
			if (CanHaveRelic)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "/";
				}
				text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Rite");
			}
			return text;
		}

		public string GetReqFormatting()
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
					text += "/";
				}
				text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Magic");
			}
			if (CanHaveRelic)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "/";
				}
				text += ArcaneOdysseyMod.Instance.CustomLocalization("ScrollTiers.Relic");
			}
			return text;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.AddTooltip(new(Mod, "ScrollTier", ArcaneOdysseyMod.Instance.CustomLocalization($"ScrollTiers.{Tier}", GetTierFormatting()).Value));
			if (!HasCorrectImbue)
			{
				tooltips.AddTooltip(new(Mod, "ScrollReq", ArcaneOdysseyMod.Instance.CustomLocalization($"ScrollTiers.NeedsImbue", GetReqFormatting()).Value));
			}
		}

		public virtual bool ExtraConditionsForImbue(Imbuable imbue) => true;

		public bool HasCorrectImbue => Item.CanHaveImbue(Imbue) && Imbue is not null;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			_ = SkillName;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EmptyScroll>();
			ArcaneOdysseyMod.Sets.showItemTypeTooltip[Type] = false;
		}
	}
}
