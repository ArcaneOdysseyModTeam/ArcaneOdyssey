using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Crystal
{
	public class SapphireVariant : MagicVariant
	{
		public override Color Colour => Color.Blue;
		public override int Imbue => AOUtils.ImbuableID<CrystalMagic>();
	}
}
