using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
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
			if (++gore.frameCounter >= 10)
			{
				if (++gore.frame > gore.numFrames)
				{
					gore.frame = 0;
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
