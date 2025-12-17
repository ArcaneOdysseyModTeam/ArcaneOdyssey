using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritBlast : SpiritProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 5 * 60;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
