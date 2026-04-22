using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class SpearThrow : PlayerProjectile
	{
		public override string Texture => AOUtils.GetTexture<BronzeSpear>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 60;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			Projectile.velocity.Y += 0.13f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}
	}
}
