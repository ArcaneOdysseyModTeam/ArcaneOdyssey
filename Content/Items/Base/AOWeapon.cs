using ArcaneOdyssey.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOWeapon : AOBaseItem, IImbuable
	{
		public void ActivateAbility(Player player, bool passive)
		{
			if (Ability.HasValue)
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, Ability.Value.Colour, Ability.Value.Name + "!", !passive);
				}
			}
		}

		public WeaponAbility? Ability 
		{ 
			get
			{
				var ab = new WeaponAbility
				{
					Colour = GetColour()
				};
				if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability.DisplayName") && Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability.Description"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability.DisplayName").Value;
					ab.Description = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability.Description").Value;
					return ab;
				}
				else if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability").Value;
					ab.Description = null;
					return ab;
				}
				return null;
			} 
		}

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

		public override ItemType? ItemCategory => ItemType.Weapon;

		public virtual WeaponType WeaponsType => WeaponType.Normal;

		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }
		public Imbuable SecondImbue { get => Item.ArcaneOdyssey()?.SecondImbue; set => Item.ArcaneOdyssey().SecondImbue = value; }

		public bool? BenifitsFromScrollStats => Item.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public virtual bool CanBeAffected => true;

		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;
		public abstract int AOValue { get; }
		public abstract AOItemTiers AOWeaponTier { get; }
		public virtual Debuff? WeaponDebuff => Debuff.Create<AOBleed>(5 * 60);
		public abstract Color Colour { get; }

		public Color GetColour()
		{
			return Imbue?.GetColour(Colour) ?? Colour;
		}

		public virtual SoundStyle UseSound => SoundID.Item71;


		/// <summary>
		/// Leave null for neutral, true for cold, false for hot
		/// </summary>
		public virtual bool? Cold => null;

		public override void SetStaticDefaults()
		{
			if (WeaponsType == WeaponType.Strength)
				ItemID.Sets.UsesBetterMeleeItemLocation[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = (27 * AOSpeed.FlipFloat()).Round();
			Item.knockBack = 4.5f * (AOSize * AOSize);
			Item.scale = AOSize;
			Item.value = AOUtils.GalleonToCopper(AOValue);
			Item.UseSound = UseSound with { Pitch = (AOSpeed * AOSpeed).MultiToPercent().Clamp(-1, 1) };
			Item.damage = (int)Math.Round(AOUtils.WeaponDamage(AOWeaponTier) * AODamage);
			Item.DamageType = DamageClass.Melee;
		}
	}
}