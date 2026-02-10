using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Lost
{
	public class DarknessArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, VisualCentre - Main.screenPosition, new(0, texture.Width() * Projectile.frame, texture.Width(), texture.Width()), Color.Lerp(lightColor, Color.Transparent, DarknessBlast.TransparencyLerp), Projectile.rotation, new(texture.Width() / 2f), Projectile.scale, SpriteEffects.None);
			}
		}
	}
}
