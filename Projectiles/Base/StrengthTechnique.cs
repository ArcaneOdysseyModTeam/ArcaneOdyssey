using ArcaneOdyssey.Imbues.FightingStyles.Normal;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class StrengthTechnique : PlayerProjectile
	{
		public override Debuff? ProjectileDebuff => null;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var oglight = lightColor;
			lightColor = Imbue?.Colour.MultiplyRGB(lightColor) ?? lightColor;
			lightColor = SecondImbue?.Colour.MultiplyRGB(oglight) ?? lightColor;
			return base.PreDraw(ref lightColor);
		}

		public override bool PreAI()
		{
			Imbue ??= ModContent.GetInstance<BasicCombat>();
			if (Main.myPlayer == Projectile.owner && Imbue?.CanBeWet == false && Projectile.wet)
			{
				Kill();
				return false;
			}
			return true;
		}
	}
}
