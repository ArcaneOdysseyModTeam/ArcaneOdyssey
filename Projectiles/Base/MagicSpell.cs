using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Linq;

namespace ArcaneOdyssey.Projectiles.Base
{
	public interface IMagicSpell : ILocalizedModType
	{
		// sets any projectile to count as a magic spell!
	}
	public abstract class MagicSpell : PlayerProjectile, IMagicSpell
	{
		public override Debuff? ProjectileDebuff => null;

		public virtual bool DrawWithImbueColours => false;

		public void GetDrawColour(ref Color lightColor)
		{
			if (Imbue is not null)
			{
				if (ImbueID.Sets.Bright[Imbue.ID])
				{
					lightColor = Color.White;
				}
				if (DrawWithImbueColours || (Imbue is MagicType magic && ImbueID.Sets.HasVariants[magic.ID] && !ImbueID.Sets.VariantsUseSprites[magic.ID]))
				{
					lightColor = Imbue.Colour.MultiplyRGB(lightColor);
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			GetDrawColour(ref lightColor);
			return base.PreDraw(ref lightColor);
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
