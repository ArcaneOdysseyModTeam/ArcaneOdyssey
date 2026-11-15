using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.Items.Consumable.SealedChests
{
	public class UnlockedDarkSealedChest : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Mystic;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.OpenableBag[Type] = true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(new GalleonsRule(Value));
			itemLoot.Add(AOUtils.Common<LostEmptyScroll>(2));
			itemLoot.Add(new CommonDrop(ItemID.GreaterHealingPotion, 1, 3, 9));
			// misc items like ores ect
			// add items like dark sea building blocks?
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.maxStack = AOPlayer.MaxSealedChests;
		}

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode; // dev mode only right now

		public override int Value => 90;

		public override bool CanRightClick() => true;
	}
}
