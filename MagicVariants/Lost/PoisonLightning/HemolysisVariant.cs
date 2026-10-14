using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Lost.PoisonLightning
{
	public class HemolysisVariant : MagicVariant<PoisonLightningMagic>
	{
		public override Color? Colour2 => Color.IndianRed;
		public override Color Colour => new(107, 7, 0);
	}
}
