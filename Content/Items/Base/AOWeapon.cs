using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;
using Terraria.ID;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Graphics;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOWeapon : ModItem
	{
        public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;
		public virtual int AOValue => 0;
		public virtual int AORarity => AORarities.Common;
		public virtual int AOWeaponTier => AOWeaponTiers.Old;
		public virtual AOMagic? CurrentImbue => null;
		public virtual AODebuff? WeaponDebuff => new(BuffID.Bleeding, 5 * 60);

		public virtual void SetDefaultsWeapon() { }

		public override void SetDefaults()
		{
			Item.useTime = 27;
			Item.knockBack = 4.5f;
			Item.rare = AORarity;
			Item.value = GalleonToCopper(AOValue, Item.rare);
			Item.autoReuse = true;
			Item.useAnimation = 27;
			Item.damage = (int)WeaponDamage(AOWeaponTier);
			Item.DamageType = DamageClass.Melee;
			SetDefaultsWeapon();
		}
	}

	public abstract class AOMagic : ModItem
	{
		public virtual float AOImbueSpeed => .9f;
		public virtual float AOImbueSize => .9f;
		public virtual float AOImbueDamage => .9f;
        public virtual float AOMagicSpeed => AOImbueSpeed;
        public virtual float AOMagicSize => AOImbueSize;
        public virtual float AOMagicDamage => AOImbueDamage;
        public virtual int MagicTier => AOMagicTier.Normal;
		public virtual AODebuff? MagicDebuff => null;
        public virtual AODebuff? MagicDebuff2 => null; // used for having freezing and frozen on a single magic ect
        public virtual MagicEffects? Effects => null;
		public virtual string? ColourCode => null;
		public virtual CombinedDebuff[]? combinedDebuffs => null;
		
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

		public virtual void SpawningDust(Vector2 spawnlocation, float attacksize = 1f /* Explosions are larger than tiny blasts lol */) { }
        public virtual void LingeringDust(Vector2 spawnlocation, float attacksize = 1f /* Explosions are larger than tiny blasts lol */) { }
    }
}
