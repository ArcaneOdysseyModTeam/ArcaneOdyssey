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
        public new const float AOSpeed = .9f;
        public new const float AOSize = 1.25f;
        public new const float AODamage = 1f;
        public const int AOWeaponTier = AOWeaponTiers.Excellent;
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.timeLeft = 99999;
        }

        public override void AI2()
        {
            // called every frame i think
        }

        public override void EffectBeforeSpin(Player player, float spintime)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage, 0f, Projectile.owner, ai1: MathHelper.TwoPi * 2f / spintime * player.direction);
        }
    }
}
