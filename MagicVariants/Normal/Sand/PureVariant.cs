using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Normal.Sand
{
	public class PureVariant : MagicVariant<SandMagic>
	{
		public override void BuffVisuals(ref int dust, ref Color drawColour, ref Color dustColour)
		{
			dust = DustID.Sand;
		}
	}
}
