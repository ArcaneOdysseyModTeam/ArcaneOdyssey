using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class SpiritProjectile : AOPlayerProjectile, IImbuable
	{
		public override string LocalizationCategory => "Imbues.Relics.Projectiles";
		public override AODebuffRequirement? Debuff => null;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = ModContent.GetInstance<OracleDamage>();
			Projectile.friendly = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Owner.MinionAttackTargetNPC = target.whoAmI;
		}
	}
}
