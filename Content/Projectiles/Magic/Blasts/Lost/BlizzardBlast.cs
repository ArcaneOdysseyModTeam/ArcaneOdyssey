using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost
{
	public class BlizzardBlast : BlastSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public const int OverlayFrames = 7;

		public override bool PreDraw(ref Color lightColor)
		{

			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Overlay", out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Vector2.Zero) * (Projectile.Size / 2f * Projectile.scale)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Width()), lightColor, Projectile.velocity.ToRotation(), new(texture.Width() / 2f), Projectile.scale * .9f, SpriteEffects.None);
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
				if (++overlayFrame >= OverlayFrames)
				{
					overlayFrame = 0;
				}
			}
		}
	}
}
