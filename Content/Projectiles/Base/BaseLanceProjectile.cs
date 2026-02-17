using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseLanceProjectile : AOPlayerProjectile
	{
		public const float Speed = BaseSpearProjectile.Speed;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Type] = true;
			ProjectileID.Sets.DismountsPlayersOnHit[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = AOUtils.TrueMeleeNoSpeed();
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
			Projectile.width = Projectile.height = 90;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
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
				Projectile.velocity.Normalize();
			}

			Owner.ChangeDir(Projectile.direction);

			Owner.heldProj = Projectile.whoAmI;
			Owner.itemTime = Owner.itemAnimation;
			Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (Projectile.velocity * Projectile.ai[1]);



			if (Owner.itemAnimation > Owner.itemAnimationMax / 2)
			{
				Projectile.Opacity = MathHelper.Lerp(0, 1f, 2f - (Owner.itemAnimation / ((float)Owner.itemAnimationMax / 2)));
				Projectile.ai[1] += Speed / (Projectile.extraUpdates + 1f);
			}
			else if (Owner.channel)
			{
				Projectile.Opacity = 1f;
				Owner.itemAnimation = Owner.itemTime = Owner.itemAnimationMax / 2;
			}
			else
			{
				Projectile.ai[1] -= Speed / (Projectile.extraUpdates + 1f);
				Projectile.Opacity = MathHelper.Lerp(0, 1f, (Owner.itemAnimation / (float)Owner.itemAnimationMax));
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver2 * Projectile.spriteDirection) - MathHelper.PiOver4;

			if (Owner.ItemAnimationEndingOrEnded)
			{
				Projectile.Kill();
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.Knockback *= Owner.velocity.Length() / 7f;
			modifiers.SourceDamage *= 0.1f + Owner.velocity.Length() / 7f * 0.9f;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float rotationFactor = Projectile.rotation + MathHelper.PiOver4;
			float scaleFactor = 95f;
			float widthMultiplier = 23f;
			float collisionPoint = 0f;

			Rectangle lanceHitboxBounds = AOUtils.ScaleRectangleNotRef(new(0, 0, 300, 300), Projectile.scale);

			lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
			lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;
			Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * scaleFactor;

			if (lanceHitboxBounds.Intersects(targetHitbox) && Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
			{
				return true;
			}
			return false;
		}
	}
}
