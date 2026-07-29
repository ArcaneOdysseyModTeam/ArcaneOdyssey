using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class InfiniteMana : ImbueGimmick
	{
		public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
		{
			mult *= 0f;
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult *= 0f;
		}
	}
}
