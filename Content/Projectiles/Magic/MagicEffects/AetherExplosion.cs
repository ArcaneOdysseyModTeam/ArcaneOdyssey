using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Magic.MagicEffects
{
    public class AetherExplosion : AOPlayerProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.height = Projectile.width = 128;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.Center = Projectile.position;
            Projectile.penetrate = -1;
            BaseScale = .75f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_Parent { Entity: Projectile projectile })
            {
                BaseScale = (projectile.width + projectile.height) / 2f / Projectile.width;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 13;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 8)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Type])
                {
                    Kill();
                }
            }
        }
    }
}
