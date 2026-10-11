using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Light
{
	public class StarlightVariant : MagicVariant<LightMagic>
	{
		public override Color Colour => ModContent.GetInstance<LightMagic>().ImbueColour;
	}
}
