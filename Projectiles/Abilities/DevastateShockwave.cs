using ArcaneOdyssey.Projectiles.Base;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class DevastateShockwave : PlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 12;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 600;
			Projectile.height = 200;
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.Bottom = Projectile.Center;
				if (!Main.dedServ)
				{
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(20f), ApplyKnockback(6f), 20, ApplyKnockback(300f), FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
			}
			if (++Projectile.frameCounter >= ApplySpeed(6f, true))
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
					return;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour.MultiplyRGB(lightColor) ?? lightColor;
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			var frame = Sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
			// draw from the bottom!
			Main.EntitySpriteDraw(Sprite, Projectile.Bottom - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(frame.Width / 2f, frame.Height), Projectile.scale, mode);
			return false;
		}
	}
}
