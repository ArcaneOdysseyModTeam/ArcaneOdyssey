using ArcaneOdyssey.Content.Buffs.DOT;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AORangedOrMeleeWeapon : AOBaseItem, ILocalizedModType, IImbuable
	{
		public override ItemType? ItemCategory => ItemType.Weapon;
		public override string LocalizationCategory => "Items.Weapons";

		public virtual WeaponType WeaponsType => WeaponType.Normal;

		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }
		public bool? BenifitsFromScrollStats => Item.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public virtual bool CanBeAffected => true;

		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;
		public abstract int AOValue { get; }
		public abstract AOItemTiers AOWeaponTier { get; }
		public virtual AODebuffRequirement? WeaponDebuff => new(ModContent.BuffType<AOBleed>(), 5 * 60);
		public virtual WeaponAbility? Ability => null;
		public virtual SoundStyle UseSound => SoundID.Item71;


		/// <summary>
		/// Leave null for neutral, true for cold, false for hot
		/// </summary>
		public virtual bool? Cold => null;

		public override void SetStaticDefaults()
		{
			if (Ability.HasValue)
				Ability.Value.GenerateTooltip();
			if (WeaponsType == WeaponType.Strength)
				ItemID.Sets.UsesBetterMeleeItemLocation[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = (27 * AOSpeed.FlipFloat()).Round();
			Item.knockBack = 4.5f * AOSize;
			Item.scale = AOSize;
			Item.value = GalleonToCopper(AOValue);
			Item.UseSound = UseSound with { Pitch = AOSpeed.MultiToPercent().Clamp(-1, 1) };
			Item.damage = (int)Math.Round(WeaponDamage(AOWeaponTier) * AODamage);
			Item.DamageType = DamageClass.Melee;
		}
	}
}