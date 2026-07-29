using ArcaneOdyssey.Projectiles.Base;
using System;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class AshCloud : PlayerProjectile
	{
		public override Debuff? ProjectileDebuff => null;

		public override float Size => 5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 25;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Generic;
			Projectile.rotation = Main.rand.NextFloat(-MathHelper.TwoPi, MathHelper.TwoPi);
			Projectile.noEnchantmentVisuals = true;
		}

		public override void AI()
		{
			Projectile.Opacity = .5f * ((Projectile.timeLeft + 1) / 120f);
			Projectile.rotation += MathHelper.PiOver4 / 60f * Math.Sign(Projectile.rotation);
			Projectile.velocity = Projectile.rotation.ToRotationVector2() * .75f;
		}

		public override void SetStaticDefaults()
		{
			ArcaneOdysseyMod.Sets.imbueEffect[Type] = true;
		}
	}
}
