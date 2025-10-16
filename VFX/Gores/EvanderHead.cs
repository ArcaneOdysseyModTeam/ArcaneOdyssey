using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Gores
{
	public class EvanderHead : ModGore
	{
		public override void SetStaticDefaults()
		{
			ChildSafety.SafeGore[Type] = false;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.sticky = true;
			gore.timeLeft = 120;
		}
	}
}
