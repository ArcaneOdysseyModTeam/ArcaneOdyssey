using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
    public abstract class FightingStyleBarred : FightingStyle
    {
		public int BarValue = 0;
		public abstract float MaxImbueSpeed { get; }
		public abstract float MaxImbueDamage { get; }
		public abstract float MaxImbueSize { get; }
		public abstract float MinImbueSpeed { get; }
		public abstract float MinImbueDamage { get; }
		public abstract float MinImbueSize { get; }
		public abstract float MaxScrollSpeed { get; }
		public abstract float MaxScrollDamage { get; }
		public abstract float MaxScrollSize { get; }
		public abstract float MinScrollSpeed { get; }
		public abstract float MinScrollDamage { get; }
		public abstract float MinScrollSize { get; }

		public override float AOImbueDamage { get => MathHelper.Lerp(MinImbueDamage, MaxImbueDamage, BarValue / 100f); }
		public override float AOScrollDamage { get => MathHelper.Lerp(MinScrollDamage, MaxScrollDamage, BarValue / 100f); }
		public override float AOImbueSpeed { get => MathHelper.Lerp(MinImbueSpeed, MaxImbueSpeed, BarValue / 100f); }
		public override float AOScrollSpeed { get => MathHelper.Lerp(MinScrollSpeed, MaxScrollSpeed, BarValue / 100f); }
		public override float AOImbueSize { get => MathHelper.Lerp(MinImbueSize, MaxImbueSize, BarValue / 100f); }
		public override float AOScrollSize { get => MathHelper.Lerp(MinScrollSize, MaxScrollSize, BarValue / 100f); }
	}
}
