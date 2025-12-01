using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class DarknessCannon : CannonSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(Texture + "_Pulse", out var texture))
			{
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition, new(0, texture.Width() * Projectile.frame, texture.Width(), texture.Width()), lightColor with { A = (byte)(lightColor.A * 0.5f) }, Projectile.rotation, new(texture.Width() / 2f), Projectile.scale, SpriteEffects.None);
			}
		}
	}
}
