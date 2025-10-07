using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class SpiritBlast : SpiritProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.alpha = (255*.25f).Round();
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (!Main.dedServ)
			{
				for (float i = 0; i > 20; i--)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IcyMerman, (MathHelper.Pi / 20 * i).ToRotationVector2().X, (MathHelper.Pi / 20 * i).ToRotationVector2().Y);
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
