using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class StrengthTechnique : AOPlayerProjectile, ILocalizedModType
    {
        public override string LocalizationCategory => "FightingStyles.Techniques";
        public override AOUtils.AODebuffRequirement? Debuff => null;
        public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
		}
	}
}
