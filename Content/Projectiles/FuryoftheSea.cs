using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class FuryoftheSea : ModProjectile
	{
        public const float AOSpeed = .9f;
        public const float AOSize = 1.25f;
        public const float AODamage = 1f;
        public const int AOWeaponTier = AOWeaponTiers.Excellent;
        

        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = (int)WeaponDamage(AOWeaponTier);
            Projectile.knockBack = 4.5f;
            Projectile.scale = AOSize;
            Projectile.timeLeft = 900;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override void AI()
        {
            Projectile.scale += .1f;
            Projectile.localAI[0] = 1f;
            if (Projectile.localAI[0] == 1f)
            {
                Dust dust = Main.dust[Dust.NewDust(Projectile.Center, 20, 40, DustID.Water, 0, 0, 100, default, 1f)];
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * -2;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.HasBuff(BuffID.Wet))
            {
                target.AddBuff(BuffID.Wet, 60 * 5);
                SoundEngine.PlaySound(SoundID.Splash, Projectile.position);
            }
        }
    }
}
