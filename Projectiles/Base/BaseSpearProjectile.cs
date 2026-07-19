namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class BaseSpearProjectile : PlayerProjectile
	{
		public const float SpearSpeed = 3.65f;

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
			Projectile.tileCollide = false;
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
			Projectile.spriteDirection = Projectile.direction;

			Owner.heldProj = Projectile.whoAmI;
			Owner.itemTime = Owner.itemAnimation;
			Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (Projectile.velocity * Projectile.ai[1]);

			if (Owner.itemAnimation < Owner.itemAnimationMax / 2)
			{
				Projectile.ai[1] -= SpearSpeed * Projectile.scale;
				if (Projectile.localAI[0] == 0f)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
					Projectile.localAI[0] = 1f;
					EffectBeforeReelBack();
				}
			}
			else
			{
				Projectile.ai[1] += SpearSpeed * Projectile.scale;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Owner.ItemAnimationEndingOrEnded && Projectile.owner == Main.myPlayer)
			{
				Kill();
			}
		}


		public virtual void EffectBeforeReelBack() { }
	}
}
