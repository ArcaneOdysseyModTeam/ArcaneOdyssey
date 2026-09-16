using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Relics
{
	public class SpiritHound : SpiritProjectile
	{
		public int TileTimer = 0;
		public override float Speed => .9f;

		public const int TimeLeftMax = 60 * 5;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 84;
			Projectile.width = 104;
			Projectile.AverageDimensions();
			Projectile.penetrate = 2;
			Projectile.localNPCHitCooldown = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = TimeLeftMax;
			Projectile.ignoreWater = true;
			Projectile.Opacity = .75f;
			Projectile.ArmorPenetration += 5;
		}

		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;

			if (TileTimer > 0)
				TileTimer--;

			if (Projectile.timeLeft == (TimeLeftMax / 2))
			{
				Projectile.penetrate--;
				Imbue?.KillEffects(Projectile.Hitbox);
			}

			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
			if (Projectile.penetrate == Projectile.maxPenetrate)
			{
				Imbue?.KillEffects(target.Hitbox.Scaled(4f));
				SecondImbue?.KillEffects(target.Hitbox.Scaled(3f));
				Projectile.timeLeft -= TimeLeftMax / 2;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (TileTimer <= 0)
			{
				Projectile.penetrate--;
				Imbue?.KillEffects(Projectile.Hitbox);
				SecondImbue?.KillEffects(Projectile.Hitbox);
			}
			if (TileTimer < 60 && TileTimer > 0)
			{
				return true;
			}
			Projectile.velocity = oldVelocity;
			Projectile.position = Projectile.oldPosition;
			TileTimer = 65;
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var texture))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, new(0, 0, texture.Width(), texture.Height()), Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2f, Projectile.scale, mode);
			}
		}
	}
}
