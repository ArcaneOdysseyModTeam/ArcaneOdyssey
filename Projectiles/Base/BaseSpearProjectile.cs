using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class BaseSpearProjectile : PlayerProjectile
	{
		public abstract ItemTiers AOWeaponTier { get; }
		public const float SpearSpeed = 3.7f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = true;
			width = height /= 4;
			return Projectile.ai[2] == 2; // do not kill projectile on tile collide unless thrown
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[2] == 2)
				Kill();
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				if (Projectile.ai[2] == 2) // throwing
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

			if (Projectile.ai[2] == 2) // throwing
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
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
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (Projectile.velocity * Projectile.ai[1]);

				if (Owner.itemAnimation < Owner.itemAnimationMax / 2)
				{
					Projectile.ai[1] -= SpearSpeed / (Projectile.extraUpdates + 1f);
					if (Projectile.localAI[0] == 0f)
					{
						if (Projectile.owner == Main.myPlayer)
						{
							Projectile.netUpdate = true;
							Projectile.netSpam = 0;
						}
						Projectile.localAI[0] = 1f;
						EffectBeforeReelBack();
					}
				}
				else
				{
					Projectile.ai[1] += SpearSpeed / (Projectile.extraUpdates + 1f);
				}

				Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * Projectile.spriteDirection) - MathHelper.PiOver4;
				if (Owner.ItemAnimationEndingOrEnded)
				{
					Kill();
				}
			}
		}

		public virtual void EffectBeforeReelBack() { }
	}
}
