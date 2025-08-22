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
        public virtual void AI2() { }
		public override void AI()
		{
            DustVelocity = Vector2.Zero;
			killDust = false;
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.AOPlayer();
			originalItem ??= player.HeldItem;
			player.heldProj = Projectile.whoAmI;
			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true);
			Projectile.direction = 1;

			float extramulti = 1f;
			if (thisMagic is not null)
			{
				extramulti = thisMagic.AOImbueSpeed.FlipFloat();
			}

            float spintime = 25 * AOSpeed.FlipFloat() * 2 * extramulti;
            Vector2 expectedDirection = player.SafeDirectionTo(Main.MouseWorld);
			Projectile.velocity = 25 * AOSpeed * expectedDirection;
            player.direction = (expectedDirection.X > 0f).ToDirectionInt();


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

            if (Projectile.ai[1] >= 600)
			{
				Projectile.ai[1] = 0f;
				EffectBeforeSpin(player, spintime);
            }
			
			else
			{
				Projectile.ai[1] += MathHelper.Pi / (MathHelper.TwoPi * 2f / spintime);
			}

            Projectile.rotation += MathHelper.TwoPi * 2f / spintime * player.direction;
			// remember that rotation is in radians, meaning pi is actually what you use (pi is a 360)

			player.itemRotation = Main.MouseScreen.ToRotation() * player.direction;
            player.itemTime = player.itemAnimation = 2;
            AI2();
        }

		public virtual void EffectBeforeSpin(Player player, float spintime) { }

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Player player = Main.player[Projectile.owner];
			AOPlayer playah = player.AOPlayer();
			Projectile.scale = BaseScale.GetValueOrDefault(2f) * (originalItem.ModItem is AOWeapon weap ? weap.AOSize : 1) * (thisMagic is not null ? thisMagic.AOImbueSize : 1);
            hitbox.Width = hitbox.Height = (int)(BaseScale * hitbox.Height * (originalItem.ModItem is AOWeapon weap2 ? weap2.AOSize : 1) * (thisMagic is not null ? thisMagic.AOImbueSize : 1));
		}
	}
}
