using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Berserker.Effects
{
	public class PowderExplosion : PlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 50;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (++Projectile.ai[0] >= 60)
			{
				if (!Main.dedServ)
				{
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				for (int n = 0; n < 10; n++)
				{
					Imbue?.ExplosionEffects(Projectile.Center);
					Imbue?.ExplosionEffects(Projectile.Center);
					SecondImbue?.ExplosionEffects(Projectile.Center);
				}
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.Kill();
				}
			}
		}

		public override bool? CanDamage() => Projectile.ai[0] >= 59;

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
