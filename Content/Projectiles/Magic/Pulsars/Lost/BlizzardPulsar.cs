using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class BlizzardPulsar : PulsarSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override bool PreDraw(ref Color lightColor)
		{

			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Overlay", out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, VisualCentre - (Projectile.velocity.SafeNormalize(Vector2.Zero) * (Projectile.Size / 2f * Projectile.scale)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Width()), lightColor, Projectile.velocity.ToRotation(), new(texture.Width() / 2f), Projectile.scale * .9f, SpriteEffects.None);
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
