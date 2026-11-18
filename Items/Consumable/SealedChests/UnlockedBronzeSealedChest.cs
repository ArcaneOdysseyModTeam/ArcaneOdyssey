using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.Items.Consumable.SealedChests
{
	public class UnlockedBronzeSealedChest : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.OpenableBag[Type] = true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(new GalleonsRule(Value));
			itemLoot.Add(AOUtils.Common<Vindicator>(60));
			itemLoot.Add(AOUtils.Common<CommonScroll>(10));
			itemLoot.Add(new CommonDrop(ItemID.LesserHealingPotion, 1, 3, 9));
			// misc items like ores ect
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.maxStack = AOPlayer.MaxSealedChests;
		}

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode; // dev mode only right now

		public override int Value => 30;

		public override bool CanRightClick() => true;
	}
}
