using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AORangedOrMeleeWeapon : AOBaseItem, ILocalizedModType
	{
		public override string LocalizationCategory => "StandardWeapons";

		public abstract float AOSpeed { get; }
		public abstract float AOSize { get; }
		public abstract float AODamage { get; }
		public abstract int AOValue { get; }
		public override ItemType ItemType => ItemType.Weapon;
		public abstract AOItemTiers AOWeaponTier { get; }
		public virtual AODebuffRequirement? WeaponDebuff => new(ModContent.BuffType<AOBleed>(), 5 * 60);
		public virtual WeaponAbility? Ability => null;


		/// <summary>
		/// Leave null for neutral, true for cold, false for hot
		/// </summary>
		public virtual bool? Cold => null;


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
			Item.UseSound = SoundID.Item71 with { Pitch = AOSpeed.MultiToPercent().Clamp(-1, 1) };
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