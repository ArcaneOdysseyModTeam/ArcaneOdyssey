using ArcaneOdyssey.Content.Projectiles.Base;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
    public class BronzeTriastaProjectile : BaseSpearProjectile
    {
        public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.extraUpdates = 10;
        }
    }
}
