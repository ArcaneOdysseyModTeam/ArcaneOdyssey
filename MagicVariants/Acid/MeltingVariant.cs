using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Acid
{
	public class MeltingVariant : MagicVariant<AcidMagic>
	{
		public override Color Colour => ModContent.GetInstance<AcidMagic>().ImbueColour;
	}
}
