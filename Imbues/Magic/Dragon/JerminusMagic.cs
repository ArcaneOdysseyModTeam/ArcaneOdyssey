using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Imbues.Magic.Dragon
{
	public class JerminusMagic : MagicType
	{
		public override Color ImbueColour => new(255, 0, 0);
		public override float ScrollSpeed => 3f;
		public override float ScrollSize => 3.5f;
		public override bool ImmuneDash => true;
		public override float ScrollDamage => .2f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Dragon;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Trauma>()];

		public override int BlastFrames => 3;

		public override void RegisterMutations()
		{

		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Imperial;
	}
}
