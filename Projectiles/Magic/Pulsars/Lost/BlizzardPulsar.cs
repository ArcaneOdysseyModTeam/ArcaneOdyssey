using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic.Blasts.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Lost
{
	public class BlizzardPulsar : PulsarSpell
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
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
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
