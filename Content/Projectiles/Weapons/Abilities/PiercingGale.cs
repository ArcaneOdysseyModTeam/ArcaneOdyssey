using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class PiercingGale : AOPlayerProjectile
	{
		public override AOUtils.AODebuffRequirement? Debuff => null;
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 60 * (Projectile.extraUpdates + 1);
			Projectile.DamageType = DamageClass.Melee;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				Projectile.velocity /= Projectile.extraUpdates+1;
			}
			Projectile.rotation += (MathHelper.Pi / 60)/Projectile.extraUpdates + 1;

			var dust = DustID.RainbowTorch;
			if (!Main.dedServ)
			{
				for (float i = 0; i < 19; i++)
				{
					var centre = ((MathHelper.PiOver4 / 19 * i) + Projectile.rotation).ToRotationVector2() * 20;
					var dust1 = Dust.NewDustPerfect(centre + Projectile.Center, dust, -(centre/15), 150, Scale: 1f);
					dust1.noLight = true;
					dust1.noGravity = true;
				}
				for (float i = 0; i < 19; i++)
				{
					var centre = (MathHelper.TwoPi / 19 * i).ToRotationVector2() * 20;
					var dust2 = Dust.NewDustPerfect(centre + Projectile.Center, dust, Projectile.velocity, 150, Scale: .5f);
					dust2.noLight = true;
					dust2.noGravity = true;
				}
				var dust3 = Dust.NewDustPerfect(Projectile.Center, dust, Vector2.Zero, 150, Scale: 1.5f);
				dust3.noLight = true;
				dust3.noGravity = true;
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
