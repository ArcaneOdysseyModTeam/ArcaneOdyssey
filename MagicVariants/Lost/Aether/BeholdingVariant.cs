using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneOdyssey.MagicVariants.Lost.Aether
{
	public class BeholdingVariant : MagicVariant<AetherMagic>
	{
		public override Color Colour => Color.BlueViolet;
		public override Color? Colour2 => Color.Purple;
	}
}
