using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class DarknessPulsar : PulsarSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Pulse", out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, new(0, texture.Width() * Projectile.frame, texture.Width(), texture.Width()), lightColor with { A = (byte)(lightColor.A * .5f) }, Projectile.rotation, new(texture.Width()), Projectile.scale, SpriteEffects.None);
			}
		}
	}
}
