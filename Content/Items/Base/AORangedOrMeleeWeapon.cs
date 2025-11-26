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

		public Imbuable Imbue { get => Item.ArcaneOdyssey().Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		public virtual bool CanHaveImbue => true;

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
        }

		/// <summary>
		/// Leave null for regular items, true for arcanium, false for strength
		/// </summary>
		public virtual bool? Arcanium => null;

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

		/// <summary>
		/// arcanium/strength weapons is checked here
		/// </summary>
		/// <param name="player">the player, dumbass</param>
		/// <returns></returns>
		public override bool CanUseItem(Player player)
		{
			if (Arcanium.HasValue)
			{
				if (Item.TryGetImbue(out Imbuable imbue))
				{
					if (Arcanium.Value)
					{
						return imbue is AOMagic;
					}
					else
					{
						return imbue is FightingStyle;
					}
				}
				return false;
			}
			return true;
		}
	}
}