using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneOdyssey.MagicVariants.Lost.PoisonLightning
{
	public class HemolysisVariant : MagicVariant<PoisonLightningMagic>
	{
		public override Color? Colour2 => Color.IndianRed;
		public override Color Colour => new(107, 7, 0);
	}
}
