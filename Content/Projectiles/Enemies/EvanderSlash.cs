using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class EvanderSlash : ModProjectile
	{
		public override string Texture => typeof(ColossalCleave).FullName.Replace('.', '/');

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage = 25;
			Projectile.timeLeft = 60 * 3;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = 234;
			Projectile.knockBack = 4.5f;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 3;
		}

		public override void AI()
		{
			Projectile.localAI[0]++;
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}

			if (++Projectile.frameCounter > 6)
			{
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			if (++Projectile.localAI[0] >= 30 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
				for (int n = 0; n < 10; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * 15f, (Main.rand.NextFloat() - 0.5f) * 15f, 255 / 2, default, 3f)];
					spawnedDust.noGravity = true;
				}
			}

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1] = 1;
			}

			if (Projectile.ai[1] != 0)
			{
				Projectile.alpha += 255 / 30;
				Projectile.ai[2] += .075f;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = width = 1;
			fallThrough = true;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 30;
			return false;
		}

		public override bool? CanDamage()
		{
			return Projectile.ai[2] < 1;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.ai[2] = 1;
		}
	}
}
