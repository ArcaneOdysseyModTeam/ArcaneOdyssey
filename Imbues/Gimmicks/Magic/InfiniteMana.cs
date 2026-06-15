using ArcaneOdyssey.Imbues.Base;
using Terraria;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class InfiniteMana : ImbueGimmick
	{
		public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
		{
			mult *= 0f;
		}
	}
}
