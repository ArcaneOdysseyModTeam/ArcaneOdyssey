using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritRaincloud : SpiritProjectile
	{
		public override float AOSize => 1.5f;

		public override bool? CanDamage() => Projectile.timeLeft <= (MaxTimeLeft - 60);

		public const int MaxTimeLeft = (60 * 10) + 60;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.DamageType = DamageClass.Summon;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = MaxTimeLeft;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 7;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Rectangle fakebox = AOUtils.ScaleRectangleNotRef(new(Projectile.Hitbox.Center.X - 190, Projectile.Hitbox.Center.Y, 190 * 2, 700), Imbue.AOScrollSize * (SecondImbue?.AOScrollSize ?? 1f), 1, 2);
			return targetHitbox.Intersects(fakebox);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			if (Projectile.timeLeft <= (MaxTimeLeft - 60))
			{
				Projectile.velocity = Vector2.Zero;
				AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(2f)), ModContent.ProjectileType<SpiritRaindrop>(), Projectile.damage / 10, 0f, Projectile.owner, Imbue, SecondImbue, true);
			}


			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				SoundEngine.PlaySound(SecondImbue?.ImbueSound, Projectile.Center);
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}
	}
}
