using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
    public class SeismicSlashRock : AOPlayerProjectile
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 120;
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            BaseScale = 2f;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.TwoPi / 40f * Projectile.direction;
        }
    }
}
