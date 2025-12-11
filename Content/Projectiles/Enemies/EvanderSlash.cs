using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class EvanderSlash : ModProjectile
	{
		//public override float AOSpeed => .65f;
		//public override float AOSize => 1.2f;
		//public override float AODamage => 1.15f;
		//public override SoundStyle? DebuffApplySound => SoundID.NPCHit42;

		//public AOWeaponTiers AOWeaponTier = AOWeaponTiers.Good;

		public override void SetDefaults()
		{
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
			if (Projectile.timeLeft < 30)
			{
				Projectile.alpha = 255 / Projectile.timeLeft;
				Projectile.ai[0] += .075f;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}

			if (Projectile.timeLeft % 6 == 0)
			{
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			if (Projectile.localAI[0] > 20 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
				for (int n = 0; n < 3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(Projectile.Center, 1, 1, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * 15f, (Main.rand.NextFloat() - 0.5f) * 15f, 255 / 2, default, 3f)];
					spawnedDust.noGravity = true;
				}
			}
			Projectile.localAI[0]++;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = width = 1;
			fallThrough = true;
			return true;
		}

		public override bool? CanDamage()
		{
			return Projectile.ai[0] < 1;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 30;
			Projectile.ai[0] = 1;
			return false;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.ai[0] = 1;
		}
	}
}
