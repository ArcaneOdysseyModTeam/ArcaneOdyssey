using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
    public class ColossalCleave : AOPlayerProjectile
    {
        public override float AOSpeed => .65f;
        public override float AOSize => 1.2f;
        public override float AODamage => 1.15f;
        public override SoundStyle? DebuffApplySound => SoundID.NPCHit42;

        public AOWeaponTiers AOWeaponTier = AOWeaponTiers.Good;

        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.alpha = (int)(225 * .1f);
            Projectile.DamageType = DamageClass.Melee;
            Projectile.damage = (int)WeaponDamage(AOWeaponTier);
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
            Projectile.height = 234;
            Projectile.width = 74;
            Projectile.knockBack = 4.5f;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = 1;
                Projectile.netUpdate = true;
            }

            Projectile.position += Projectile.velocity;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.localAI[0] > 60 && !Main.dedServ)
            {
                Projectile.localAI[0] = 0;
                Imbue.ExplosionEffects(Projectile);
            }
            Projectile.localAI[0]++;
        }
    }
}
