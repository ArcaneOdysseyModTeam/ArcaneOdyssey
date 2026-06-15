using ArcaneOdyssey.Imbues.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Gimmicks.Magic
{
	public class InfiniteWoodWands : ImbueGimmick
	{
		public override void InventoryEffects(Item item, Player player)
		{
			if (ArcaneOdysseyMod.Sets.woodWand[item.type])
			{
				item.tileWand = ItemID.None;
			}
		}

		public override void NoInventoryEffects(Item item, Player player)
		{
			if (ArcaneOdysseyMod.Sets.woodWand[item.type])
			{
				item.tileWand = ArcaneOdysseyMod.Sets.wandWoodType[item.type];
			}
		}
	}
}
