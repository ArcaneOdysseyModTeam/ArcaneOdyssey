using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class ShotTechnique : StrengthTechnique
	{
		public const int DustCount = 30;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 30;
			Projectile.extraUpdates = 20;
			Projectile.timeLeft = 150;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] > 2)
			{
				Projectile.localAI[0] = 0;
				for (float i = 0; i < DustCount; i++)
				{
					var centre2 = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width / 2);
					var dust2 = Dust.NewDustPerfect(centre2 + Projectile.Center, DustID.RainbowTorch, (-centre2) / 5, 150, Scale: 1f);
					dust2.noLight = true;
					dust2.alpha = 250;
					dust2.noGravity = true;
					Imbue?.LingeringEffects(Projectile);
				}
			}
			Projectile.localAI[0]++;
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height /= 2;
            fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
