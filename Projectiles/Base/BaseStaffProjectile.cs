using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class BaseStaffProjectile : AOPlayerProjectile
	{
		public override Debuff? ProjectileDebuff => null;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = AOUtils.TrueMeleeNoSpeed();
			Projectile.height = Projectile.width = 175;
			Projectile.friendly = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
		}

		public override void OnKill(int timeLeft)
		{
			Owner.channel = false;
		}

		public override void AI()
		{
			Owner.heldProj = Projectile.whoAmI;
			Projectile.Center = Owner.RotatedRelativePoint(Owner.RotatedRelativePoint(Owner.MountedCenter), true);
			Projectile.direction = 1;

			float spintime = 25f * AOSpeed.FlipFloat() * 2f * (Imbue?.AOImbueSpeed.FlipFloat() ?? 1f);
			Vector2 expectedDirection = Owner.SafeDirectionTo(Main.MouseWorld);
			Owner.ChangeDir((expectedDirection.X > 0f).ToDirectionInt());


			if (Projectile.ai[0] == 0f)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.ai[0] = 1f;
			}

			if (Owner.dead || !Owner.channel)
			{
				Kill();
				Owner.reuseDelay = 2;
				return;
			}

			Projectile.timeLeft = 2;

			if (Projectile.ai[1] >= 600 || Projectile.ai[1] <= -600)
			{
				Projectile.ai[1] = 0f;
				EffectBeforeSpin(Owner);
			}

			else
			{
				Projectile.ai[1] += MathHelper.Pi / (MathHelper.TwoPi * 2f / (25f * AOSpeed.FlipFloat() * 2f * (Imbue?.AOImbueSpeed ?? 1f)));
			}

			Projectile.rotation += MathHelper.TwoPi * 2f / spintime * Owner.direction;
			Owner.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
			Owner.itemTime = Owner.itemAnimation = 2;
		}

		public virtual void EffectBeforeSpin(Player player) { }
	}
}
