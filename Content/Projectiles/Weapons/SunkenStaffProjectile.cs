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
	public class SunkenStaffProjectile : BaseStaffProjectile
	{
		public override float AOSpeed => .9f;
		public override float AOSize => 1.25f;
		public override float AODamage => 1f;
		public int AOWeaponTier = AOWeaponTiers.Excellent;
		public override AODebuff Debuff => new(BuffID.Wet, 600);
		public override SoundStyle? DebuffApplySound => SoundID.Splash;

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 40;
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

		public override void AI2()
		{   if (Main.netMode != NetmodeID.Server) {
			// dust
			Random rnd = new Random();
			for(int dustCountInt = 0;dustCountInt<2;dustCountInt++) {
				Dust.NewDust(Projectile.Center,3,3,DustID.Water,50f*(0.5f-rnd.NextSingle()),50f*(0.5f-rnd.NextSingle()),255,default,1.3f);
			}
			}
		}

		public override void EffectBeforeSpin(Player player, float spintime)
		{
			if (Main.player[Projectile.owner] == Main.clientPlayer)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage, 0f, Projectile.owner, ai1: MathHelper.TwoPi * 2f / spintime * player.direction);
		}
	}
}
