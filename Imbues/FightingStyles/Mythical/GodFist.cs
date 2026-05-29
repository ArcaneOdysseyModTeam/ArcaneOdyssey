using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Imbues.FightingStyles.Mythical
{
	public class GodFist : FightingStyle
	{
		public override string Texture => AOUtils.GetTexture<SailorStyle>();
		public override bool ImmuneDash => true;
		public override float ImbueDamage => .8f;
		public override float ImbueSize => 1.5f;
		public override float ImbueSpeed => 1.6f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;
		public override Color ImbueColour => new(255, 224, 228);
	}
}
