using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic.Blasts.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Lost
{
	public class BlizzardArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var OverlayFrames = BlizzardBlast.OverlayFrames;
			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Overlay", out var texture))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				if (Proj1Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj1.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj1.Width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
					Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				if (Proj2Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj2.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj2.Width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
					Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				if (Proj3Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj3.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj3.Width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
					Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				if (Proj4Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj4.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj4.Width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
					Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				return false;
			}
			return base.PreDraw(ref lightColor);
		}

		private int overlayFrame = 0;
		private int overlayFrameCounter = 0;
		public override void Animate()
		{
			base.Animate();
			if (overlayFrameCounter++ > 2)
			{
				overlayFrameCounter = 0;
				if (++overlayFrame >= BlizzardBlast.OverlayFrames)
				{
					overlayFrame = 0;
				}
			}
		}
	}
}
