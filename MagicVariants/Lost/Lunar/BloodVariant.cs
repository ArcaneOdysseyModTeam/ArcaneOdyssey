using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Lost.Lunar
{
	public class BloodVariant : MagicVariant<LunarMagic>
	{
		public override Color Colour => new(105, 0, 16);

		public override Color? Colour2 => new(255, 115, 115);

		public override void BuffVisuals(ref int dust, ref Color drawColour, ref Color dustColour)
		{
			dust = DustID.Water_BloodMoon;
		}

		public override void BuffVisuals(ref int[] dust, ref Color drawColour, ref Color dustColour)
		{
			dust = [DustID.Water_BloodMoon, DustID.VampireHeal, DustID.CrimsonTorch];
		}
	}
}
