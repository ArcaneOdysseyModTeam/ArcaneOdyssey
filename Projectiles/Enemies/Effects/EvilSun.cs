using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic.Effects;

namespace ArcaneOdyssey.Projectiles.Enemies.Effects
{
	public class EvilSun : BaseProjectile
	{
		public override string Texture => AOUtils.GetTexture<ProminenceProjectile>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.width = Projectile.height = 20;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 3;
			Projectile.ArmorPenetration += 10;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<Melting>(), 120, false);
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.wet && !(Projectile.lavaWet || Projectile.honeyWet || Projectile.shimmerWet))
			{
				Dust.NewDust(Projectile.Center, 0, 0, DustID.Smoke);
			}
			else
			{
				Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.White;
			return base.PreDraw(ref lightColor);
		}
	}
}
