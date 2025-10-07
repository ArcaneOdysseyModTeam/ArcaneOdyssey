using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
    public class SailorFist : FightingStyle
    {
        public override Color ImbueColour => Color.White;

		public override float AOImbueDamage => 1.075f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.06f;
		public override float AOScrollDamage => .925f;
		public override float AOScrollSize => 1f;
	}
}
