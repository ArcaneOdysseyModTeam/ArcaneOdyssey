using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class RisingTideProjectile : PlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 50;
		}
		public override float Size => 2.5f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 34 * 2;
			Projectile.width = 104 * 2;
			Projectile.DamageType = AOUtils.TrueMelee();
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Dust.NewDust(Projectile.Center, 0, Projectile.height, DustID.Water, Main.rand.NextBool().ToDirectionInt() * 10f, Scale: 3f);
			Projectile.velocity = Vector2.UnitY * -1f * Owner.gravDir;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.timeLeft % 5 == 0)
			{
				Projectile.Center = Owner.Center + (Projectile.velocity * 5f);

				if (Projectile.frame++ >= (Main.projFrames[Type] + 1))
				{
					Projectile.Kill();
				}
			}
			Projectile.Opacity = 1f - (Projectile.frame / (float)Main.projFrames[Type]);
		}

		public override Debuff? ProjectileDebuff => Debuff.Create<Soaked>();

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				if (k % 5 == 0)
				{
					Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
					var colour2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					var frame = Projectile.frame - (k / 5);
					Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, Sprite.Frame(1, Main.projFrames[Type], 0, frame), colour2, Projectile.oldRot[k], new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale + (k * .05f), SpriteEffects.None);
				}
			}
			return false;
		}
	}
}
