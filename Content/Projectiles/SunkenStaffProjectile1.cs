using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class SunkenStaffProjectile1 : ModProjectile
    {
        public float AOSpeed = .9f;
        public float AOSize = 1.25f;
        public float AODamage = 1f;
        public int AOWeaponTier = AOWeaponTiers.Excellent;

        public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.Spear);
            Projectile.damage = WeaponDamage(AODamage, AOWeaponTier);
            Projectile.knockBack = WeaponSize(AOSize, AOWeaponTier);
            Projectile.scale = WeaponSize(AOSize, AOWeaponTier);
            Projectile.timeLeft = Main.player[Projectile.owner].itemAnimationMax;
        }
    }
}
