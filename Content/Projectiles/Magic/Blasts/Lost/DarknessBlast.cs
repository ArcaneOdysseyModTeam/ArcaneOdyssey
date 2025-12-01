using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost
{
	public class DarknessBlast : BlastSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public static float TransparencyLerp => .75f;

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Pulse", out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, new(0, texture.Width() * Projectile.frame, texture.Width(), texture.Width()), Color.Lerp(lightColor, Color.Transparent, TransparencyLerp), Projectile.rotation, new(texture.Width() / 2f), Projectile.scale, SpriteEffects.None);
			}
		}
	}
}
