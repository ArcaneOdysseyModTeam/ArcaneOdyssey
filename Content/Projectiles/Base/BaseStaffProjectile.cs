using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseStaffProjectile : AOPlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = TrueMeleeNoSpeed();
			Projectile.knockBack = 4.5f;
			Projectile.height = Projectile.width = 120;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.ArcaneOdyssey();
			player.heldProj = Projectile.whoAmI;
			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true);
			Projectile.direction = 1;

			float spintime = 25f * AOSpeed.FlipFloat() * 2f * (Imbue?.AOImbueSpeed.FlipFloat() ?? 1f);
			Vector2 expectedDirection = player.SafeDirectionTo(Main.MouseWorld);
			player.ChangeDir((expectedDirection.X > 0f).ToDirectionInt());


			if (Projectile.ai[0] == 0f)
			{
				Projectile.netUpdate = true;
				Projectile.ai[0] = 1f;
			}

			if (player.dead || !player.channel)
			{
				Projectile.Kill();
				player.reuseDelay = 2;
				return;
			}

			if (Projectile.ai[1] >= 600 || Projectile.ai[1] <= -600)
			{
				Projectile.ai[1] = 0f;
				EffectBeforeSpin(player);
			}

			else
			{
				Projectile.ai[1] += MathHelper.Pi / (MathHelper.TwoPi * 2f / (25f * AOSpeed.FlipFloat() * 2f * (Imbue?.AOImbueSpeed ?? 1f)));
			}

			Projectile.rotation += MathHelper.TwoPi * 2f / spintime * player.direction;
			player.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
			player.itemTime = player.itemAnimation = 2;
		}

		public virtual void EffectBeforeSpin(Player player) { }
	}
}
