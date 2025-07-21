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
		public virtual AODebuff? WeaponDebuff => null;

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
		public virtual int MagicTier => AOMagicTier.Normal;
		public virtual AODebuff? MagicDebuff => new AODebuff(BuffID.Bleeding, 5*60); // defaults to bleed ofc
		public virtual MagicEffects Effects => null;
		public virtual string? ColourCode => null;
		
		public virtual void SetDefaultsMagic() { }

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.DrinkOld;
			Item.useTime = 1;
			Item.useAnimation = 1;
			Item.noUseGraphic = true;
			SetDefaultsMagic();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanReforge() => false;
	}
}
