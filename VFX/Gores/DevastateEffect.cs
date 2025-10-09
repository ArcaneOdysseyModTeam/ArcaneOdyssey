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
	public class DevastateEffect : ModGore
	{
		public override void SetStaticDefaults()
		{
			ChildSafety.SafeGore[Type] = true;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.numFrames = 12;
			gore.frame = 0;
			gore.sticky = true;
			gore.timeLeft = 30;
			gore.alpha = 255;
		}

		public override bool Update(Gore gore)
		{
			gore.frameCounter++;
			if (gore.frameCounter >= 10)
			{
				if (gore.frame < gore.numFrames)
				{
					gore.frame++;
				}
				else
				{
					gore.active = false;
				}
				gore.frameCounter = 0;
			}
			gore.timeLeft--;
			return false;
		}
	}
}
