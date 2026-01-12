using ArcaneOdyssey.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseSpearProjectile : AOPlayerProjectile
	{
		public abstract AOItemTiers AOWeaponTier { get; }
		public const float Speed = 3.7f;
		public override AODebuffRequirement? Debuff => new(ModContent.BuffType<AOBleed>(), 50 * 5);

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
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
					Projectile.velocity *= 3 / (Projectile.extraUpdates + 1f);
					Projectile.timeLeft = 60 * (Projectile.extraUpdates + 1);
				}
				else
				{
					Projectile.velocity.Normalize();
				}
			}

			Owner.ChangeDir(Projectile.direction);

			if (Projectile.ai[2] != 0) // throwing
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				Projectile.timeLeft = 2;
				Projectile.velocity.Y += 0.13f;
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}
			else
			{
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemTime = Owner.itemAnimation;
				Projectile.Center = Owner.Center + (Projectile.velocity * Projectile.ai[1]);

				if (Owner.itemAnimation < Owner.itemAnimationMax / 2)
				{
					Projectile.ai[1] -= Speed / (Projectile.extraUpdates + 1f);
					if (Projectile.localAI[0] == 0f)
					{
						Projectile.netUpdate = true;
						Projectile.localAI[0] = 1f;
						EffectBeforeReelBack();
					}
				}
				else
				{
					Projectile.ai[1] += Speed / (Projectile.extraUpdates + 1f);
				}

				Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * Projectile.spriteDirection) - MathHelper.PiOver4;
				if (Owner.itemAnimation <= 2)
				{
					Projectile.Kill();
					Owner.reuseDelay = 2;
				}
			}
		}

		public virtual void EffectBeforeReelBack() { }
	}
}
