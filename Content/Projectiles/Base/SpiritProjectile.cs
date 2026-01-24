using System;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class SpiritProjectile : AOPlayerProjectile, IImbuable
	{
		public override string LocalizationCategory => "Imbues.Relics.Projectiles";
		public override AODebuffRequirement? Debuff => null;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = OracleDamage.Instance;
			Projectile.friendly = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Owner.MinionAttackTargetNPC = target.whoAmI;
			if (Projectile.TryGetOwner(out var owner))
			{
				owner.ArcaneOdyssey()?.TrySpiritLifesteal(Math.Clamp(Projectile.originalDamage / 5, 1, 20), false);
			}
		}
	}
}
