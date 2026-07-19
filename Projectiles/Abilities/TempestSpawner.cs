using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class TempestSpawner : PlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override bool? CanDamage() => false;
		public override void AI()
		{
			AOPlayerOwner.HeavySkillActive = true;
			AOPlayerOwner.CanMoveOnGround = false;
			Owner.itemAnimation = Owner.itemTime = Owner.itemAnimationMax / 4;
			if (Main.myPlayer == Projectile.owner)
			{
				if (Projectile.timeLeft % 10 == 0)
				{
					Owner.direction *= -1;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Tempest>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				}
			}
		}

		public override bool CanHaveImbueVFX => false;

		public override float Size => 2.5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 2;
			Projectile.DamageType = AOUtils.TrueMelee();
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60 * 2;
		}
	}
}
