using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class BreathtakerProjectile : StrengthTechnique
	{
		public override string Texture => AOUtils.BlankTexture;

		public override bool CanHaveImbueVFX => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = Player.defaultHeight;
			Projectile.extraUpdates = 1000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.ownerHitCheck = true;
			Projectile.noEnchantmentVisuals = true;
		}

		public override bool? CanCutTiles() => false;

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.ArcaneOdyssey().LowerDefense(2, target.Hitbox);
		}

		public override void AI()
		{
			if (Projectile.Distance(Owner.Center) > ApplySpeed(180f))
			{
				if (Projectile.owner == Main.myPlayer)
				{
					Kill();
				}
				return;
			}
			Projectile.velocity = Owner.velocity.SafeNormalize(Projectile.velocity);
		}
	}
}
