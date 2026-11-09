using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Lost.Lunar
{
	public class MidnightVariant : MagicVariant<LunarMagic>
	{
		public override void BuffVisuals(ref int dust, ref Color drawColour, ref Color dustColour)
		{
			dust = DustID.Water_GlowingMushroom;
		}

		public override void BuffVisuals(ref int[] dust, ref Color drawColour, ref Color dustColour)
		{
			dust = [DustID.Water_GlowingMushroom, DustID.RedsWingsRun, DustID.MushroomTorch];
		}
	}
}
