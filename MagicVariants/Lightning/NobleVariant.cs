using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Lightning
{
	public class NobleVariant : MagicVariant<LightningMagic>
	{
		public override Color Colour => ModContent.GetInstance<LightningMagic>().ImbueColour;
	}
}
