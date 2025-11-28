using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
    public class SeismicSlashRock : AOPlayerProjectile
    {
        public override float AOSize => 2f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 120;
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.TwoPi / 40f * Projectile.direction;
        }
    }
}
