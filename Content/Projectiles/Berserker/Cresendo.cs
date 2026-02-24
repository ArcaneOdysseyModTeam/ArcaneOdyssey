using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class Cresendo : StrengthTechnique
	{		
		public override string Texture => AOUtils.GetTexture<SparrowThrust>();

		public override bool CanHaveImbueVFX => false;
		public Color Colour => Imbue?.GetColour(Color.White) ?? Color.White;
		public int MaxTime => ApplyImbueSpeed(60, true).Round();
		public int TrueMaxTime => MaxTime + (100 * 60);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 186;
			Projectile.timeLeft = TrueMaxTime;
			Projectile.extraUpdates = 100;
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
			if (Projectile.timeLeft > (TrueMaxTime - MaxTime))
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				oldvelo = Projectile.velocity;
				Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			}
			else
			{
				//if (Projectile.ai[0] == 0)
				//{
				//	for (int i = 0; i < 5; i++)
				//	{
				//		Imbue?.ExplosionEffects(Projectile.Center);
				//	}
				//	Projectile.ai[0] = 1;
				//}
				if (++Projectile.frameCounter > ((TrueMaxTime - MaxTime) / 10f))
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame > Main.projFrames[Type])
					{
						Projectile.frame = 0;
					}
				}
				Projectile.velocity = Vector2.Zero;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var realkmax = 18;
			for (int k = realkmax; k >= 0; k--)
			{
				Vector2 drawPos = VisualCentre - (oldvelo * k * (7f / (realkmax / 9f))) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Colour * (1f - ((realkmax - k) / (float)realkmax)));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), colour2, Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
