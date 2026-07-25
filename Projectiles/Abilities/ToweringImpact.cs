using ArcaneOdyssey.Projectiles.Base;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class ToweringImpact : PlayerProjectile
	{
		public override string Texture => AOUtils.SlashTexture;

		public override void SetDefaults()
		{
			Projectile.scale = 1.5f;
			base.SetDefaults();
			Projectile.timeLeft = 60;
			Projectile.friendly = true;
			Projectile.width = 74;
			Projectile.height = 234;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.DamageType = AOUtils.TrueMelee();
			Projectile.penetrate = -1;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
			Projectile.position.X += 45 * Projectile.scale * Owner.direction;
			Projectile.velocity = Vector2.Zero;
			if (Owner.direction == -1)
			{
				Projectile.rotation = MathHelper.Pi;
			}
			if (AOPlayerOwner.grounded)
			{
				Projectile.position.Y -= Projectile.height / 3;
			}
			else
			{
				Projectile.position.Y += Projectile.height / 3;
			}
			if (!Main.dedServ)
			{
				PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}



		public override void AI()
		{
			if (Owner.ItemAnimationActive)
			{
				AOPlayerOwner.HeavySkillActive = true;
				Imbue?.ExplosionEffects(Projectile.Center, 2f);
				SecondImbue?.ExplosionEffects(Projectile.Center, 2f);
			}
			Projectile.alpha += 255 / 60;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour.MultiplyRGB(lightColor) ?? lightColor;
			return base.PreDraw(ref lightColor);
		}
	}
}
