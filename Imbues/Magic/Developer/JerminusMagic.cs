using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Imbues.Magic.Developer
{
	public class JerminusMagic : MagicType
	{
		public override Color ImbueColour => new(255, 0, 0);
		public override float ScrollSpeed => 3f;
		public override float ScrollSize => 3.5f;
		public override float DashSpeed => 1.4f;
		public override float ScrollDamage => .2f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Developer;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Trauma>()];

		public override int BlastFrames => 1;

		public override void RegisterMutations()
		{
			
		}
	}
}
