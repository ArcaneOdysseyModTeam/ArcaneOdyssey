using Terraria.DataStructures;
using Terraria.GameContent;

namespace ArcaneOdyssey.Gores.Elius
{
	public class EliusLeg : ModGore
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
