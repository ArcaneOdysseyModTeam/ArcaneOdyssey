using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class ShockwaveSmash : StrengthTechnique
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 100;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = AOUtils.TrueMeleeNoSpeed();
			Projectile.localNPCHitCooldown = -1;
			Projectile.Opacity = .5f;
		}

		public override float AOSize => 2.5f;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.Center = Owner.Center + (Projectile.velocity * 20f);
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.velocity = Vector2.Zero;
				Projectile.ai[0] = 1;
			}

			if (++Projectile.frameCounter > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
				}
			}
		}
	}
}
