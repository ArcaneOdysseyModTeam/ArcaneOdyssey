using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Magic;

namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public class JerminusMagic : MagicType
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<ReverseGravity>();
		public override Color ImbueColour => new(255, 0, 0);
		public override float ScrollSpeed => 3f;
		public override float ScrollSize => 3.5f;
		public override bool ImmuneDash => true;
		public override float ScrollDamage => .2f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Trauma>()];

		public override int BlastFrames => 3;

		public override void RegisterMutations()
		{

		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Imperial;
	}
}
