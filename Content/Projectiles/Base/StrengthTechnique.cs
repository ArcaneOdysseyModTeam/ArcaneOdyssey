using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class StrengthTechnique : AOPlayerProjectile, ILocalizedModType
	{
		public override string LocalizationCategory => "Imbues.FightingStyles.Projectiles";
		public override AODebuffRequirement? Debuff => null;
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
		}
	}
}
