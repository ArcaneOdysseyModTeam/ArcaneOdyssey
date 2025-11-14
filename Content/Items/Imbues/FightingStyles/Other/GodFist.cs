using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Other
{
    public class GodFist : FightingStyle
    {
        public override float DashSpeed => 1.5f;
        public override float AOImbueDamage => 3f;
        public override float AOImbueSize => 5f;
        public override float AOImbueSpeed => 2f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
    }
}
