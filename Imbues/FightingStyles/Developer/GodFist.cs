using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Imbues.FightingStyles.Developer
{
	public class GodFist : FightingStyle
	{
		public override string Texture => AOUtils.GetTexture<SailorStyle>();
		public override float DashSpeed => 1.4f;
		public override float AOImbueDamage => .8f;
		public override float AOImbueSize => 1.5f;
		public override float AOImbueSpeed => 1.6f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
		public override Color ImbueColour => new(255, 224, 228);
	}
}
