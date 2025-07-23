using System;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseStaffProjectile : AOPlayerProjectile
	{
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.GetModPlayer<AOPlayer>();
			originalItem = player.HeldItem;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = (int)(FlipFloat(AOSpeed)*60);
			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, true);
			Projectile.direction = 1;

            float spintime = 25 * AOSpeed * 2;
            Vector2 expectedDirection = SafeDirectionTo(player, Main.MouseWorld);
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
				EffectBeforeSpin();
            }
			
			else
			{
				Projectile.ai[1] += MathHelper.Pi / (MathHelper.TwoPi * 2f / spintime);
			}

            Projectile.rotation += MathHelper.TwoPi * 2f / spintime * player.direction;
            // remember that rotation is in radians, meaning pi is actually what you use (pi is a 360)
        }

		public virtual void EffectBeforeSpin() { }

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Player player = Main.player[Projectile.owner];
			AOPlayer playah = player.GetModPlayer<AOPlayer>();
			Projectile.scale = 2f * (originalItem.ModItem is AOWeapon weap ? weap.AOSize : 1) * (playah.imbue is not null ? playah.imbue.AOImbueSize : 1);
			hitbox.Height = (int)(hitbox.Height * (originalItem.ModItem is AOWeapon weap2 ? weap2.AOSize : 1) * (playah.imbue is not null ? playah.imbue.AOImbueSize : 1));
			hitbox.Width = hitbox.Height;
		}
	}
}
