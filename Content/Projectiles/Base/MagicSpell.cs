using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class MagicSpell : AOPlayerProjectile, ILocalizedModType
	{
		public override string LocalizationCategory => "Magic.Spells";
		public override AODebuffRequirement? Debuff => null;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}
	}
}
