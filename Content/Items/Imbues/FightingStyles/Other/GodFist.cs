using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Developer
{
	public class GodFist : FightingStyle
	{
		public override string Texture => typeof(SailorStyle).FullName.Replace('.', '/');
		public override float DashSpeed => 1.5f;
		public override float AOImbueDamage => 3f;
		public override float AOImbueSize => 5f;
		public override float AOImbueSpeed => 2f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
	}
}
