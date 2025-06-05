using System;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class FuryOfTheSea : ModProjectile
	{
        public float AOSpeed = .9f;
        public float AOSize = 1.25f;
        public float AODamage = 1f;
        public int AOWeaponTier = AOWeaponTiers.Excellent;

        public override void SetDefaults()
        {

        }
    }
}
