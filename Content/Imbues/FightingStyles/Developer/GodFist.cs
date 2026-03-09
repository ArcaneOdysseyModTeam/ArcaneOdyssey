using ArcaneOdyssey.Content.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Imbues.FightingStyles.Developer
{
	public class GodFist : FightingStyle
	{
		public override string Texture => AOUtils.GetTexture<SailorStyle>();
		public override float DashSpeed => 1.4f;
		public override float AOImbueDamage => .8f;
		public override float AOImbueSize => 1.5f;
		public override float AOImbueSpeed => 1.6f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
	}
}
