using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class Selino : StrengthTechnique
	{
		public override string Texture => AOUtils.SlashTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 300;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = AOUtils.TrueMeleeNoSpeed();
			Projectile.localNPCHitCooldown = -1;
			Projectile.timeLeft = 60;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				NetUpdate();
				Projectile.Center = Owner.Center + (Projectile.velocity * 20f);
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.velocity = Vector2.Zero;
				Projectile.ai[0] = 1;
			}

			Projectile.alpha += 255 / 60;

			//Imbue?.ExplosionEffects(Projectile.Center, .8f);
			//SecondImbue?.ExplosionEffects(Projectile.Center, .8f);
		}
	}
}
