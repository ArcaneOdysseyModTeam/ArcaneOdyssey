using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Gores
{
	public class DuskCape : ModGore
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
