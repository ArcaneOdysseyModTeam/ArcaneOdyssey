using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class Scroll : AOBaseItem, IImbuable
	{
		public float ApplyScrollSpeed(float value, bool flipfloat = false)
		{
			if (Imbue is not null)
			{
				if (!flipfloat)
				{
					value *= Imbue.AOScrollSpeed;
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed;
				}
				else
				{
					value *= Imbue.AOScrollSpeed.FlipFloat();
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed.FlipFloat();
				}
			}
			return value;
		}

		public float ApplyImbueSpeed(float value, bool flipfloat = false)
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
			return value;
		}
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

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EmptyScroll>();
		}
	}
}
