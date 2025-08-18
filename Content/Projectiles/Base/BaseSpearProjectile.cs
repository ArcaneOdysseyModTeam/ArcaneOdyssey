using System;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseSpearProjectile : AOPlayerProjectile
	{
		public float Speed = 3f;

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.GetModPlayer<AOPlayer>();
			originalItem = player.HeldItem;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.itemTime = player.itemAnimation;
			Projectile.Center = player.RotatedRelativePoint(player.MountedCenter, addGfxOffY: false);
			Projectile.position += Projectile.velocity * Projectile.ai[0];
			if (Projectile.ai[0] == 0f)
			{
				Projectile.netUpdate = true;
			}

			if (player.itemAnimation < player.itemAnimationMax / 2)
			{
				Projectile.ai[0] -= Speed/3;
				if (Projectile.localAI[0] == 0f) 
				{
					Projectile.localAI[0] = 1f;
					EffectBeforeReelBack();
				}
			}
			
			else
			{
				Projectile.ai[0] += Speed/4;
			}

			if (player.itemAnimation <= 1)
				Projectile.Kill();

			// remember that rotation is in radians, meaning pi is actually what you use (pi is a 360)
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection != -1 ? MathHelper.PiOver2 : -MathHelper.PiOver2) - (MathHelper.Pi/4); // really simple, do a 180 in the direction youre facing and correct i think
			if (player.itemAnimation == 2)
			{
				Projectile.Kill();
				player.reuseDelay = 2;
			}
		}

		public virtual void EffectBeforeReelBack() { }

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Player player = Main.player[Projectile.owner];
			AOPlayer playah = player.GetModPlayer<AOPlayer>();
			Projectile.scale = BaseScale * (originalItem.ModItem is AOWeapon weap ? weap.AOSize : 1) * (thisMagic is not null ? thisMagic.AOImbueSize : 1);
			hitbox.Height = (int)(hitbox.Height * (originalItem.ModItem is AOWeapon weap2 ? weap2.AOSize * BaseScale : BaseScale) * (thisMagic is not null ? thisMagic.AOImbueSize : 1));
			hitbox.Width = (int)(hitbox.Width * (originalItem.ModItem is AOWeapon weap3 ? weap3.AOSize * BaseScale : BaseScale) * (thisMagic is not null ? thisMagic.AOImbueSize : 1));
		}
	}
}
