using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class BlizzardCannon : CannonSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Overlay", out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Width()), lightColor, -MathHelper.PiOver2, new(texture.Width() / 2f), Projectile.scale, SpriteEffects.None);
			}
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
