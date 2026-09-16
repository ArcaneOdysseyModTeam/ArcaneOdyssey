using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class BlastSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 60;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Imbue?.UpdateProjectile(Projectile);

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override string Texture => (Mod.Name + "/" + ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? WindMagic.ID]?.Name ?? typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast")).Replace("\\", "/");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? WindMagic.ID]?.Value ?? base.Sprite;

		public static int TrailFrame => (int)(Main.GameUpdateCount / 15 % 7);

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is BlizzardMagic)
			{
				var texture = BlizzardMagic.trail;
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
			}
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]) / 2f, Projectile.scale, mode);
			return false;
		}

		public Asset<Texture2D> Glow;

		public override void PostDraw(Color lightColor)
		{
			if (AOUtils.RequestIfExists(GlowTexture, ref Glow))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				if (Glow.Height() == Sprite.Height)
				{
					var Sprite = Glow.Value;
					Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]) / 2f, Projectile.scale, mode);
				}
				else
				{
					var Sprite = Glow.Value;
					Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, Sprite.Size() / 2f, Projectile.scale, mode);
				}
			}
		}
	}
}
