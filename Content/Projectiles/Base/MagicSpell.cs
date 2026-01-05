using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class MagicSpell : AOPlayerProjectile, ILocalizedModType
	{
		public override string LocalizationCategory => "Imbues.Magic.Projectiles";
		public override AODebuffRequirement? Debuff => null;

		public string Tier
		{
			get
			{
				var split = GetType().FullName.Split('.');
				foreach (var item in split)
				{
					if (item == AOImbuableTier.Normal.ToString() || item == AOImbuableTier.Lost.ToString() || item == AOImbuableTier.Ancient.ToString() || item == AOImbuableTier.Developer.ToString())
					{
						return item;
					}
				}
				return "Any";
			}
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
		}
	}
}
