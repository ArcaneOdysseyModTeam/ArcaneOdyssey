using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class WoodenStaffProjectile : BaseStaffProjectile
	{
		public override bool? Cold => true;
		public override float AOSpeed => 1.05f;
		public override float AOSize => .9f;
		public override float AODamage => 1f;

		public AOWeaponTiers AOWeaponTier = AOWeaponTiers.Poor;

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 60;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.damage = (int)WeaponDamage(AOWeaponTier);
			Projectile.knockBack = 4.5f;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			BaseScale = 2f;
		}
	}
}
