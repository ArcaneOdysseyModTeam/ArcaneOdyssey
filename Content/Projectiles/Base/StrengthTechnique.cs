using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class StrengthTechnique : AOPlayerProjectile
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
			lightColor = Imbue?.GetColour(lightColor) ?? lightColor;
			return base.PreDraw(ref lightColor);
		}

		public override bool PreAI()
		{
			if (Main.myPlayer == Projectile.owner && (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet)))
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
