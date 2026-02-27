using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class Selino : StrengthTechnique
	{
		public override string Texture => AOUtils.SlashTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 100;
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
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.velocity.Normalize();
				Projectile.Center = Owner.Center + (Projectile.velocity * 20f);
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[0] = 1;
			}

			Projectile.alpha += 255 / 60;

			//Imbue?.ExplosionEffects(Projectile.Center, .8f);
			//SecondImbue?.ExplosionEffects(Projectile.Center, .8f);
		}
	}
}
