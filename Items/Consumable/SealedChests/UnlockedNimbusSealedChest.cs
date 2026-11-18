using ArcaneOdyssey.Items.Base;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.Items.Consumable.SealedChests
{
	public class UnlockedNimbusSealedChest : UnlockedBronzeSealedChest
	{
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(new GalleonsRule(Value));
			itemLoot.Add(AOUtils.Common<RareScroll>(10));
			itemLoot.Add(new CommonDrop(ItemID.HealingPotion, 1, 3, 9));
			// misc items like ores ect
		}
	}
}
