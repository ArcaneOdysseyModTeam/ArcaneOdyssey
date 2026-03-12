using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class ShotTechnique : StrengthTechnique
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 50;
			Projectile.extraUpdates = 20;
			Projectile.timeLeft = 90;
		}

		public override void AI()
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < 5; i++)
				{
					var centre2 = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * (Projectile.width / 2);
					var dust2 = AOUtils.NewDustImperfect(centre2 + Projectile.Center, DustID.BubbleBurst_White, (-centre2) / 5, 0, Imbue?.GetColour() ?? Color.White, .9f);
					dust2.noLight = true;
					dust2.noGravity = true;
				}
			}
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
				for (float i = 0; i < 15; i++)
				{
					var centre2 = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * (Projectile.width * 2);
					var dust2 = AOUtils.NewDustImperfect(centre2 + Projectile.Center, DustID.BubbleBurst_White, (-centre2) / 5, 0, Imbue?.GetColour() ?? Color.White, 1.5f);
					dust2.noLight = true;
					dust2.noGravity = true;
					Imbue?.ExplosionEffects(Projectile.Center);
				}
			}
			return base.PreKill(timeLeft);
		}
	}
}
