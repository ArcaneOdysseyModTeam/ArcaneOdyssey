using System;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseStaffProjectile : AOPlayerProjectile
	{
		public override void SetDefaults()
		{
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
			DustVelocity = Vector2.Zero;
			killDust = false;
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.ArcaneOdyssey();
			player.heldProj = Projectile.whoAmI;
			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true);
			Projectile.direction = 1;

			float extramulti = 1f;
			if (Imbue is not null)
			{
				extramulti = Imbue.AOImbueSpeed.FlipFloat();
			}

			float spintime = 25 * AOSpeed.FlipFloat() * 2 * extramulti;
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
				Projectile.ai[1] += MathHelper.Pi / (MathHelper.TwoPi * 2f / spintime);
			}

			Projectile.rotation += MathHelper.TwoPi * 2f / spintime * player.direction;
			// remember that rotation is in radians, meaning pi is actually what you use (pi is a 360)

			player.itemRotation = MathHelper.WrapAngle(Projectile.rotation); ;
			player.itemTime = player.itemAnimation = 2;
		}

		public virtual void EffectBeforeSpin(Player player) { }
	}
}
