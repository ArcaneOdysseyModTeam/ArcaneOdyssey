using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace ArcaneOdyssey.Gores.TalosGuard
{
	public class TalosGuardHead : ModGore
	{
		public override void SetStaticDefaults()
		{
			// its just a helmet
			ChildSafety.SafeGore[Type] = true;
		}

		public override string Texture => AOUtils.TerrariaItemTexture(ItemID.GladiatorHelmet);
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.timeLeft /= 2;
		}
	}
}
