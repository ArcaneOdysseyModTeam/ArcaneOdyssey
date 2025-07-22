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
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class SunkenStaffProjectile : BaseSpearProjectile
    {
        public new const float AOSpeed = .9f;
        public new const float AOSize = 1.25f;
        public new const float AODamage = 1f;
        public const int AOWeaponTier = AOWeaponTiers.Excellent;

        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 40;
            Projectile.DamageType = DamageClass.Melee;
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
        }

        public override void EffectBeforeReelBack()
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.TopRight, Projectile.velocity * 2f, ModContent.ProjectileType<FuryoftheSea>(), Projectile.damage, 0f, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Wet, 60 * 5);
        }
    }
}
