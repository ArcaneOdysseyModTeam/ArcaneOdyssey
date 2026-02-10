using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritRaindrop : SpiritProjectile
	{
		public override bool CanHaveImbueVFX => false;
		public override string Texture => AOUtils.GetTexture<SpiritBlast>();
		public override float AOSize => .15f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 90;
			Projectile.Opacity = .25f;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
		}

		public override void AI()
		{
			Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			SecondImbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				Projectile.netUpdate = true;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}

			Projectile.velocity.X *= .95f;

			Projectile.velocity.Y += 0.13f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}
	}
}
