using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class MagicSpell : AOPlayerProjectile
	{
		public override Debuff? ProjectileDebuff => null;

		public virtual bool DrawWithImbueColours => false;

		public override bool PreDraw(ref Color lightColor)
		{
			if (DrawWithImbueColours)
			{
				lightColor = Imbue?.Colour ?? Color.White;
			}
			return base.PreDraw(ref lightColor);
		}

		public virtual bool HasMagicVariant => false;

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

		public override bool PreAI()
		{
			Imbue ??= ModContent.GetInstance<WindMagic>();
			if (Main.myPlayer == Projectile.owner && Imbue?.CanBeWet == false && Projectile.wet)
			{
				Kill();
				return false;
			}
			return true;
		}
	}
}
