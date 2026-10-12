using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Normal.Wood
{
	public class ForestVariant : MagicVariant<WoodMagic>
	{
		public override Color Colour => ModContent.GetInstance<WoodMagic>().ImbueColour;
	}
}
