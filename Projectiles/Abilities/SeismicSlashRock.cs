using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class SeismicSlashRock : PlayerProjectile
	{
		public override float Size => 2f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 60;
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
