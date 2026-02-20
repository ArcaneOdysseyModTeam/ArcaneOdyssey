using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class Scroll : AOBaseItem, IImbuable, ILocalizedModType
	{
		public override bool ShowItemTypeTooltip => false;
		public override string LocalizationCategory => "Scrolls." + Tier;

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
			Item.width = 32;
			Item.height = 32;
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
			Item.DamageType = Item.DamageType.UnImbued(Item);
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
			Item.DamageType = Item.DamageType.Imbued(Imbue, Item);
		}

		public override bool CanUseItem(Player player) => Imbue is not null;

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


		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.AddTooltip(new(Mod, "ScrollTier", Mod.CustomLocalization($"ScrollTiers.{Tier}", GetTierFormatting()).Value));
		}

		public virtual bool ExtraConditionsForImbue(Imbuable imbue) => true;

		public bool HasCorrectImbue => Item.CanHaveImbue(Imbue) && Imbue is not null;
	}
}
