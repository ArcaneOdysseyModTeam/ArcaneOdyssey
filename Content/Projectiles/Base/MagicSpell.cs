using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class MagicSpell : AOPlayerProjectile, ILocalizedModType
	{
        public override string LocalizationCategory => "Spells";
        public override AOUtils.AODebuffRequirement? Debuff => null;
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}
	}
}
