using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Relics
{
	public class SpiritExplosion : SpiritProjectile
	{
		public override bool CanHaveImbueVFX => false;
		public override string Texture => AOUtils.BlankTexture;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.width = Projectile.height = 100;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.timeLeft = 30;
			SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center, null);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center, null);
				NetUpdate();
			}
			Projectile.velocity = Vector2.Zero;
			Imbue?.ExplosionEffects(Projectile.Center);
			SecondImbue?.ExplosionEffects(Projectile.Center);
		}
	}
}
