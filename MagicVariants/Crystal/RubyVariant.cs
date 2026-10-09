using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Crystal
{
	public class RubyVariant : MagicVariant
	{
		public override Color Colour => ModContent.GetInstance<CrystalMagic>().ImbueColour; // default colour
		public override int Imbue => AOUtils.ImbuableID<CrystalMagic>();
	}
}
