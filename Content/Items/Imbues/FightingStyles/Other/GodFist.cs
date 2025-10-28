using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Other
{
    public class GodFist : FightingStyle
    {
        public override float AOImbueDamage => 3f;
        public override float AOImbueSize => 5f;
        public override float AOImbueSpeed => 2f;
        public override AOUtils.AOImbuableTier ImbuableTier => AOUtils.AOImbuableTier.Developer;
    }
}
