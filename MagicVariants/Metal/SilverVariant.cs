using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneOdyssey.MagicVariants.Metal
{
	public class SilverVariant : MagicVariant<MetalMagic>
	{
		public override Color Colour => Color.White; // metal is already greyscale
	}
}
