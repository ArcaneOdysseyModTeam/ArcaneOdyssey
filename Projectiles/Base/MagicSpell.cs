using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class MagicSpell : PlayerProjectile
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

		public string Tier
		{
			get
			{
				var split = GetType().FullName.Split('.');
				foreach (var item in split)
				{
					if (item == ImbuableTiers.Normal.ToString() || item == ImbuableTiers.Lost.ToString() || item == ImbuableTiers.Ancient.ToString() || item == ImbuableTiers.Developer.ToString())
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
				return TouchingWater();
			}
			return true;
		}


		/// <summary>
		/// Override for custom behaviour on touching water
		/// <para/>By default, cancels ai and kills the projectile
		/// </summary>
		/// <returns></returns>
		public virtual bool TouchingWater()
		{
			Kill();
			return false;
		}
	}
}
