using ArcaneOdyssey.Projectiles.Base;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;

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
			Projectile.ownerHitCheck = true;
			Projectile.timeLeft = 30;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				NetUpdate();
				if (!Main.dedServ)
				{
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), Projectile.timeLeft, ApplyKnockback(500f), FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
			}
			Projectile.velocity = Vector2.Zero;
			Imbue?.ExplosionEffects(Projectile.Center, Projectile.scale);
			SecondImbue?.ExplosionEffects(Projectile.Center, Projectile.scale);
		}
	}
}
