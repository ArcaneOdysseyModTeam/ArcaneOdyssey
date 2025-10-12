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
	public class Impact : ModGore
	{
		public override void SetStaticDefaults()
		{
			ChildSafety.SafeGore[Type] = true;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.numFrames = 3;
			gore.frame = 1;
			gore.sticky = true;
			gore.timeLeft = 30;
		}

		public override bool Update(Gore gore)
		{
			gore.frameCounter++;
			if (gore.frameCounter >= 10)
			{
				if (gore.frame + 1 > 2)
				{
					gore.frame = 0;
				}
				else
				{
					gore.frame++;
				}
				gore.frameCounter = 0;
			}
			gore.timeLeft--;
			gore.alpha = 255 - (gore.timeLeft * 4);
			if (gore.timeLeft <= 0)
			{
				gore.active = false;
			}
			return false;
		}
	}
}
