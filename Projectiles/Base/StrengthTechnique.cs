using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

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
			lightColor = Imbue?.Colour ?? lightColor;
			lightColor = SecondImbue?.Colour ?? lightColor;
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
			if (Imbue is IronLeg)
			{
				Owner.position.Y -= .001f;
			}
			return true;
		}
	}
}
