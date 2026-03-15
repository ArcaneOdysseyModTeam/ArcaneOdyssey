using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class DevastateShockwave : AOPlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 12;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			if (Sprite is not null)
			{
				Projectile.width = Sprite.Width;
				Projectile.height = Sprite.Height / Main.projFrames[Type];
			}
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.friendly = true;
			Projectile.DamageType = AOUtils.TrueMelee();
		}

		public override void AI()
		{
			if (++Projectile.frameCounter >= 7)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
					return;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.GetColour() ?? Color.Orange;
			return base.PreDraw(ref lightColor);
		}
	}
}
