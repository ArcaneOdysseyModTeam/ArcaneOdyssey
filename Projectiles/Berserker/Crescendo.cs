using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class Crescendo : StrengthTechnique
	{
		public override bool CanHaveImbueVFX => false;
		public static int TravelTime => 60;
		public static int LingerTime => 100 * 60;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 186;
			Projectile.timeLeft = TravelTime + LingerTime;
			Projectile.extraUpdates = 100;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		private Vector2 oldvelo;

		public override void AI()
		{
			AOPlayerOwner.HeavySkillActive = true;
			if (Projectile.timeLeft > LingerTime)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				oldvelo = Projectile.velocity;
				Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
				SecondImbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			}
			else
			{
				if (Projectile.ai[0] == 0)
				{
					Projectile.ai[0] = 1;
					if (!Main.dedServ)
					{
						PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(20f), ApplyKnockback(6f), 20, ApplyKnockback(1000f), FullName);
						Main.instance.CameraModifiers.Add(modifier);
					}
				}
				if (++Projectile.frameCounter > ApplySpeed(LingerTime / (float)Main.projFrames[Type], true))
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame >= Main.projFrames[Type])
					{
						Kill();
					}
				}
				Projectile.velocity = Vector2.Zero;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour ?? lightColor;
			lightColor = SecondImbue?.Colour ?? lightColor;
			var realkmax = ApplySpeed(12f).Round();
			for (int k = realkmax; k >= 0; k--)
			{
				Vector2 drawPos = Projectile.Center - (oldvelo * k * (7f / (realkmax / 9f))) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(lightColor * (1f - ((realkmax - k) / (float)realkmax)));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), colour2, Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
