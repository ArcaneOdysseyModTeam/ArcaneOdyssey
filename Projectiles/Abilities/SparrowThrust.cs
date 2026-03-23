using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class SparrowThrust : PlayerProjectile
	{
		public override bool CanHaveImbueVFX => false;
		public Color Colour => Imbue?.Colour ?? Color.MediumPurple;
		public static int LingerTime => 60;
		public static int TravelTime => 100 * 60;

		public override float AOSize => 1.5f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 7;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 128;
			Projectile.friendly = true;
			Projectile.timeLeft = LingerTime + TravelTime;
			Projectile.extraUpdates = 100;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		private Vector2 oldvelo;

		public override void AI()
		{
			if (Projectile.timeLeft > TravelTime)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				oldvelo = Projectile.velocity;
				Imbue?.LingeringEffects(Projectile.Hitbox.Scaled(1f - (.75f * ((Projectile.timeLeft - TravelTime) / (float)TravelTime))), Projectile.velocity, Projectile);
			}
			else
			{
				if (Projectile.ai[0] == 0)
				{
					for (int i = 0; i < 5; i++)
					{
						Imbue?.ExplosionEffects(Vector2.Lerp(Projectile.Center, Owner.MountedCenter, .5f));
					}
					Projectile.ai[0] = 1;
				}
				if (++Projectile.frameCounter > ApplySpeed(TravelTime / Main.projFrames[Type], true))
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
			var realkmax = ApplySpeed(9f).Round();
			for (int k = realkmax; k >= 0; k--)
			{
				Vector2 drawPos = Projectile.Center - (oldvelo * k * (7f / (realkmax / 9f))) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Colour * (1f - ((realkmax - k) / (float)realkmax)));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), colour2, Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale - (Projectile.scale * .075f * k), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
