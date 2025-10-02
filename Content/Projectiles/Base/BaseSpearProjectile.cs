using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseSpearProjectile : AOPlayerProjectile
	{
		public abstract AOWeaponTiers AOWeaponTier { get; }
		public const float Speed = 3.7f;
		public override AODebuffRequirement Debuff => new(ModContent.BuffType<AOBleed>(), 50*5);

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 60;
			Projectile.knockBack = 4.5f;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
			Projectile.damage = (int)WeaponDamage(AOWeaponTier);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			width = height /= 4;
			return Projectile.ai[2] != 0; // do not kill projectile on tile collide unless thrown
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[2] != 0)
				Projectile.Kill();
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				if (Projectile.ai[2] != 0) // throwing
				{
					Projectile.velocity *= 3;
					Projectile.timeLeft = 60;
				}
				else
				{
					Projectile.velocity.Normalize();
				}
			}

			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.ArcaneOdyssey();
			player.ChangeDir(Projectile.direction);

			if (Projectile.ai[2] != 0) // throwing
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				Projectile.timeLeft = 2;
				Projectile.velocity.Y += .1f;
			}
			else
			{
				player.heldProj = Projectile.whoAmI;
				player.itemTime = player.itemAnimation;
				Projectile.Center = player.Center + (Projectile.velocity * Projectile.ai[1]);

				if (player.itemAnimation < player.itemAnimationMax / 2)
				{
					Projectile.ai[1] -= Speed;
					if (Projectile.localAI[0] == 0f)
					{
						Projectile.netUpdate = true;
						Projectile.localAI[0] = 1f;
						EffectBeforeReelBack();
					}
				}

				else
				{
					Projectile.ai[1] += Speed;
				}

				// remember that rotation is in radians, meaning pi is actually what you use (pi is a 360)
				Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * Projectile.spriteDirection) - MathHelper.PiOver4; // really simple, do a 180 in the direction youre facing and correct i think
				if (player.itemAnimation <= 2)
				{
					Projectile.Kill();
					player.reuseDelay = 2;
				}
			}
		}

		public virtual void EffectBeforeReelBack() { }
	}
}
