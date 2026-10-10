using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Crystal
{
	public class EmeraldVariant : MagicVariant
	{
		public override int Imbue => AOUtils.ImbuableID<CrystalMagic>();

		public override Color Colour => Color.Green;
	}
}
