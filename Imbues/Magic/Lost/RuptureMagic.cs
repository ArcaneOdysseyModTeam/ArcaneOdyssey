using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class RuptureMagic : MagicType
	{
		public override int BlastFrames => 4;

		public override Color ImbueColour => Color.Lime;

		public override Color ImbueColour2 => Color.Black;

		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<ExplosionMagic>();
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Reminiscent;

		public override float DashSpeed => 1.2f;

		public override float ScrollSpeed => 0.85f;
		public override float ScrollSize => 1.3f;
		public override float ScrollDamage => 0.925f;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override SoundStyle? ImbueSound => SoundID.Item14;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<CharredEffect>()];
	}
}
