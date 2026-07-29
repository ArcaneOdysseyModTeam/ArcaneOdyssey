using Terraria.DataStructures;
using Terraria.GameContent;

namespace ArcaneOdyssey.Gores
{
	public class EmptyHealthPotion : ModGore
	{
		public override void SetStaticDefaults()
		{
			ChildSafety.SafeGore[Type] = false;
		}

		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			gore.sticky = false;
			gore.timeLeft = 120;
		}
	}
}
