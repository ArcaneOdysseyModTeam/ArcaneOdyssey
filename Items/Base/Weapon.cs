using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class Weapon : BaseItem, IImbuable
	{
		public void ActivateAbility(Player player, bool passive)
		{
			if (Ability.HasValue)
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, Ability.Value.Colour, ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Exclaim", Ability.Value.Name).Value.Trim(), !passive);
				}
			}
		}

		public WeaponAbility? Ability
		{
			get
			{
				var ab = new WeaponAbility
				{
					Colour = Colour
				};
				if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability.DisplayName") && Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability.Description"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability.DisplayName").Value;
					ab.Description = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability.Description").Value;
					if (Imbue is not null)
					{
						ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", Imbue.PrettyAttackPrefix, ab.Name).Value.Trim();
					}
					if (SecondImbue is not null)
					{
						ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", SecondImbue.PrettyAttackPrefix, ab.Name).Value.Trim();
					}
					return ab;
				}
				else if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability").Value;
					ab.Description = null;
					if (Imbue is not null)
						if (Imbue is not null)
						{
							ab.Name = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Space", Imbue.PrettyAttackPrefix, ab.Name).Value.Trim();
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

		public float ApplySpeed(float value, bool flipfloat = false) => Item.ArcaneOdyssey().ApplySpeed(value, flipfloat);
		public float ApplySize(float value, bool flipfloat = false) => Item.ArcaneOdyssey().ApplySize(value, flipfloat);

		public sealed override ItemType? ItemCategory => ItemType.Weapon;

		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }
		public Imbuable SecondImbue { get => Item.ArcaneOdyssey()?.SecondImbue; set => Item.ArcaneOdyssey().SecondImbue = value; }

		public bool? BenifitsFromScrollStats => Item.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public virtual float Speed => 1f;
		public virtual float Size => 1f;
		public virtual float Damage => 1f;
		public abstract ItemTiers WeaponTier { get; }
		public virtual Debuff? WeaponDebuff => Debuff.Create<Bleeding>(5 * 60);
		public abstract Color Motif { get; }

		public Color Colour => Imbue?.Colour ?? Motif;

		public virtual SoundStyle UseSound => SoundID.Item71;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = (27 * Speed.FlipFloat().Pow()).Round();
			Item.knockBack = MathF.Round(4.5f * Size.Pow(), 2);
			Item.scale = Size.Pow();
			Item.UseSound = UseSound with { Pitch = Speed.Pow().MultiToPercent().Clamp(-1, 1) };
			Item.damage = (22 * (int)WeaponTier * Damage.Pow()).Round();
			Item.DamageType = DamageClass.Melee;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
			{
				tooltips.AddTooltip(new(Mod, "DebugSpeed", nameof(Speed) + " " + Speed));
				tooltips.AddTooltip(new(Mod, "DebugSize", nameof(Size) + " " + Size));
				tooltips.AddTooltip(new(Mod, "DebugDamage", nameof(Damage) + " " + Damage));
				tooltips.AddTooltip(new(Mod, "DebugTier", nameof(WeaponTier) + " " + ((int)WeaponTier)));
			}
		}
	}
}