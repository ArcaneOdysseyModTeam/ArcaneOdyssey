using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Phoenix
{
	public class WarPhoenixVariant : BasePhoenixVariant
	{
		public override Color Colour => ModContent.GetInstance<PhoenixMagic>().ImbueColour;
		public override Color? Colour2 => ModContent.GetInstance<PhoenixMagic>().DefaultAnimatedColour;
	}
}
