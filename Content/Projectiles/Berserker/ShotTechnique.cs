using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class ShotTechnique : StrengthTechnique
	{
		public const int DustCount = 20;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 30;
			Projectile.extraUpdates = 20;
			Projectile.timeLeft = 90;
		}

		public override void AI()
		{
			if (Projectile.localAI[0] > 2 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				for (float i = 0; i < DustCount; i++)
				{
					var centre2 = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width / 2);
					var dust2 = Dust.NewDustPerfect(centre2 + Projectile.Center, DustID.BubbleBurst_White, (-centre2) / 5, 0, Imbue is null ? default : Imbue.GetColor(), .9f);
					dust2.noLight = true;
					dust2.noGravity = true;
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

        public override bool PreKill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (float i = 0; i < DustCount; i++)
                {
                    var centre2 = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width * 2);
                    var dust2 = Dust.NewDustPerfect(centre2 + Projectile.Center, DustID.BubbleBurst_White, (-centre2) / 5, 0, Imbue is null ? default : Imbue.GetColor(), 1.5f);
                    dust2.noLight = true;
                    dust2.noGravity = true;
                    Imbue?.ExplosionEffects(Projectile);
                }
                AOUtils.SimulateAOE(Projectile.width * 2, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType, false);
            }
            return base.PreKill(timeLeft);
        }
	}
}
