using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Gimmicks.Magic
{
	public class InfiniteWoodWands : ImbueGimmick
	{
		public override void InventoryEffects(Item item, Player player)
		{
			item.tileWand = ItemID.None;
		}

		public override void NoInventoryEffects(Item item, Player player)
		{
			item.tileWand = ArcaneOdysseyMod.Sets.tileWand[item.type];
		}
	}
}
