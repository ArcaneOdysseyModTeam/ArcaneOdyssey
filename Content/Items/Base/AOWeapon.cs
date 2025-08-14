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
	public abstract class AOWeapon : ModItem
	{
		public new string LocalizationCategory => "Items.Weapons";
        public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;
		public virtual int AOValue => 0;
		public virtual int AORarity => AORarities.Common;
		public virtual int AOWeaponTier => AOWeaponTiers.Old;
		public virtual AOMagic CurrentImbue => null;
		public virtual AODebuff WeaponDebuff => new(BuffID.Bleeding, 5 * 60);

		public virtual void SetDefaultsWeapon() { }

		public override void SetDefaults()
		{
			Item.useTime = 27; // do not change, handled in GlobalItem
			Item.knockBack = 4.5f; // do not change, handled in GlobalItem
            Item.rare = AORarity;
			Item.value = GalleonToCopper(AOValue, Item.rare);
			Item.autoReuse = true;
			Item.useAnimation = 27; // do not change, handled in GlobalItem
            Item.damage = (int)WeaponDamage(AOWeaponTier);
			Item.DamageType = DamageClass.Melee;
			SetDefaultsWeapon();
		}
	}

    /// <summary>
    /// Imbue values are applied as multipliers to imbued projectiles,
    /// Magic values are applied as multipliers to projectiles created using spell scrolls
    /// </summary>
    public abstract class AOMagic : ModItem
	{
		public new string LocalizationCategory => "Items.Magics";
        public virtual float AOImbueSpeed => .9f;
		public virtual float AOImbueSize => .9f;
		public virtual float AOImbueDamage => .9f;
		public virtual float AOMagicSpeed => AOImbueSpeed;
		public virtual float AOMagicSize => AOImbueSize;
		public virtual float AOMagicDamage => AOImbueDamage;
		public virtual AOMagicTier MagicTier => AOMagicTier.Normal;
		public virtual AODebuff MagicDebuff => null;

        /// <summary>
		/// used for having freezing and frozen on a single magic ect
		/// </summary>
        public virtual AODebuff MagicDebuff2 => null; 
        public virtual MagicEffects Effects => null;
		public virtual Color MagicColour => Color.Transparent;
		public virtual CombinedDebuff[] combinedDebuffs => null;

		public virtual Dictionary<Type, int> Spells => new();
		
		public virtual void SetDefaultsMagic() { }

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.DrinkOld;
			Item.useTime = 1;
			Item.useAnimation = 1;
			Item.noUseGraphic = true;
			SetDefaultsMagic();
		}

		public override bool CanReforge() => false;

		public virtual void SpawningDust(Vector2 spawnlocation, float attacksize = 1f /* Literally just Projectile.scale */) { }
        public virtual void LingeringDust(Vector2 spawnlocation, float attacksize = 1f /* Literally just Projectile.scale */) { }
        public virtual void KillDust(Vector2 spawnlocation, float attacksize = 1f /* Literally just Projectile.scale */) { }
    }
}
