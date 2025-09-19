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
	public abstract class AOWeapon : ModItem
	{
		
        public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;
		public virtual float AODamage => 1f;
		public abstract int AOValue { get; }
		public virtual AORarities AORarity => AORarities.Common;
		public virtual AOWeaponTiers AOWeaponTier => AOWeaponTiers.Old;
		public virtual AODebuffRequirement WeaponDebuff => new(ModContent.BuffType<AOBleed>(), 5 * 60);


		/// <summary>
		/// Leave null for neutral, true for cold, false for hot
		/// </summary>
		public virtual bool? ColdWeapon => null;


		/// <summary>
		/// Leave null for regular items, true for arcanium, false for strength
		/// </summary>
		public virtual bool? Arcanium => null;

		public virtual void SetDefaultsWeapon() { }

		public override void SetDefaults()
		{
			Item.useTime = Item.useAnimation = 27; // do not multiply, handled in GlobalItem
			Item.knockBack = 4.5f; // do not change, handled in GlobalItem
            Item.rare = (int)AORarity;
			Item.value = GalleonToCopper(AOValue);
			Item.autoReuse = true;
            Item.damage = (int)WeaponDamage(AOWeaponTier);
			Item.DamageType = DamageClass.Melee;
			SetDefaultsWeapon();
		}
	}
}
