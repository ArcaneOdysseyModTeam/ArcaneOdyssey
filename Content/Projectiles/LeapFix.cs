using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class LeapFix : ModProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void AI()
		{
			Main.player[Projectile.owner].direction = (int)Projectile.ai[0];
			Projectile.Kill();
		}
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
