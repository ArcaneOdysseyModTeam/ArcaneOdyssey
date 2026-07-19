using ArcaneOdyssey.Projectiles.Base;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class DevastateShockwave : PlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 12;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 594;
			Projectile.height = 108;
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
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.Bottom = Owner.Bottom;
				if (!Main.dedServ)
				{
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(20f), ApplyKnockback(6f), 20, ApplyKnockback(300f), FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
			}
			if (++Projectile.frameCounter >= ApplySpeed(6f, true))
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
			lightColor = Imbue?.Colour ?? lightColor;
			return base.PreDraw(ref lightColor);
		}
	}
}
