using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class BasicStrike : StrengthTechnique
	{
		public override string Texture => AOUtils.GetTexture<Impact>();

		public override bool CanHaveImbueVFX => false;

		public Vector2? initPos = null;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 60;
			Projectile.timeLeft = 10;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
			DrawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Imbue?.SpawningEffects(Projectile.Hitbox, Projectile.velocity);
				Projectile.Center = Owner.Center + (Projectile.velocity.SafeNormalize(Vector2.Zero) * 10f);
				initPos = Projectile.Center;
				Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			}
			initPos ??= Projectile.Center;
			Projectile.Opacity = Projectile.timeLeft / 10f;
			Owner.heldProj = Projectile.whoAmI;
			if (++Projectile.frameCounter >= 10)
			{
				if (++Projectile.frame > Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
				Projectile.frameCounter = 0;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour ?? lightColor;
			Main.EntitySpriteDraw(Sprite, initPos.GetValueOrDefault(Projectile.Center) - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, SpriteEffects.None);
			return false;
		}
	}
}
