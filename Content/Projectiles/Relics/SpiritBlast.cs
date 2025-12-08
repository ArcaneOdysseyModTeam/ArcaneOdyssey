using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritBlast : SpiritProjectile
	{
		public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
			}

			if (!Main.dedServ)
			{
				Lighting.AddLight(Projectile.Center, TorchID.Ice);
				for (float i = 0; i < 10; i++)
				{
					Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.width, DustID.IcyMerman, Projectile.velocity.X/2, Projectile.velocity.Y/2).noGravity = true;
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

		public const int DustCount = 50;

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < DustCount; i++)
				{
					var centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width + Projectile.height);
					var dust = Dust.NewDustPerfect(Projectile.Center, DustID.IcyMerman, centre / (13 + (Main.rand.NextFloat() * 2)));
					dust.noGravity = true;
					centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width + Projectile.height);
					dust = Dust.NewDustPerfect(Projectile.Center, DustID.IcyMerman, centre / (14 + (Main.rand.NextFloat() * 2)));
					dust.noGravity = true;
					centre = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width + Projectile.height);
					dust = Dust.NewDustPerfect(Projectile.Center, DustID.IcyMerman, centre / (15 + (Main.rand.NextFloat() * 2)));
					dust.noGravity = true;
				}
				SimulateAOE(Projectile.width * 1.25f, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType);
			}
		}
	}
}
