using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.SealedChests
{
	public class DarkSealedChest : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Mystic;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.OpenableBag[Type] = true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(new GalleonsRule(90, 120));
			itemLoot.Add(AOUtils.Common<Vindicator>(6)); // move to bronze sealed later
			itemLoot.Add(AOUtils.Common<LostScroll>(2));
			itemLoot.Add(new CommonDrop(ItemID.HealingPotion, 1, 3, 9));
			// misc items like ores ect
			// add items like dark sea building blocks?
		}

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode; // dev mode only right now

		public override int Value => 200;

		public override bool CanRightClick() => true;
	}
}
