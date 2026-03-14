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
	public abstract class Scroll : AOBaseItem, IImbuable
	{
		public void ActivateAbility(Player player)
		{
			if (Ability.HasValue)
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, Ability.Value.Colour, (Ability.Value.Name + "!").Trim(), true);
				}
			}
		}

		public LocalizedText SkillName => Language.GetOrRegister(this.GetLocalizationKey("SkillName"), PrettyPrintName);


		public WeaponAbility? Ability
		{
			get
			{
				if (HasCorrectImbue)
				{
					var ab = new WeaponAbility
					{
						Colour = Imbue.GetColour(Tier switch
						{
							ScrollTier.Common => Color.White,
							ScrollTier.Rare => Color.Aqua,
							ScrollTier.Lost => Color.AliceBlue,
							_ => Color.White,
						}),
						Description = null,
						Name = SkillName.Value
					};
					if (Imbue is not FightingStyle)
					{
						ab.Name = (Imbue.PrettySpellPrefix + " " + ab.Name).Trim();
					}
					else if (SecondImbue is not null)
					{
						ab.Colour = SecondImbue.GetColour(Tier switch
						{
							ScrollTier.Common => Color.White,
							ScrollTier.Rare => Color.Aqua,
							ScrollTier.Lost => Color.AliceBlue,
							_ => Color.White,
						});
					}
					if (SecondImbue is not null)
					{
						ab.Name = (SecondImbue.PrettyAttackPrefix + " " + ab.Name).Trim();
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
							value *= Imbue.AOScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed;
						}
						else
						{
							value *= Imbue.AOScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed;
						}
						else
						{
							value *= Imbue.AOImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public bool? BenifitsFromScrollStats => Item.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public override bool ShowItemTypeTooltip => false;

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

		public abstract int AOValue { get; }

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.value = AOUtils.GalleonToCopper(AOValue);
		}

		public override void UpdateInventory(Player player)
		{
			if (HasCorrectImbue)
			{
				Item.color = Color.Lerp(Color.Transparent, Imbue.GetColour(Color.Transparent), .75f);
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
				Item.color = Color.Lerp(Color.Transparent, Imbue.GetColour(Color.Transparent), .75f);
			}
			else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player) => Imbue is not null && !Item.accessory;

		public string GetTierFormatting()
		{
			var text = "";
			if (CanHaveFS)
			{
				text += Mod.CustomLocalization("ScrollTiers.Technique");
			}
			if (CanHaveMagic)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "/";
				}
				text += Mod.CustomLocalization("ScrollTiers.Spell");
			}
			if (CanHaveRelic)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "/";
				}
				text += Mod.CustomLocalization("ScrollTiers.Rite");
			}
			return text;
		}

		public string GetReqFormatting()
		{
			var text = "";
			if (CanHaveFS)
			{
				text += Mod.CustomLocalization("ScrollTiers.FightingStyle");
			}
			if (CanHaveMagic)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "/";
				}
				text += Mod.CustomLocalization("ScrollTiers.Magic");
			}
			if (CanHaveRelic)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "/";
				}
				text += Mod.CustomLocalization("ScrollTiers.Relic");
			}
			return text;
		}


		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.AddTooltip(new(Mod, "ScrollTier", Mod.CustomLocalization($"ScrollTiers.{Tier}", GetTierFormatting()).Value));
			if (!HasCorrectImbue)
			{
				tooltips.AddTooltip(new(Mod, "ScrollReq", Mod.CustomLocalization($"ScrollTiers.NeedsImbue", GetReqFormatting()).Value));
			}
		}

		public virtual bool ExtraConditionsForImbue(Imbuable imbue) => true;

		public bool HasCorrectImbue => Item.CanHaveImbue(Imbue) && Imbue is not null;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			_ = SkillName;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CommonEmptyScroll>();
		}
	}
}
