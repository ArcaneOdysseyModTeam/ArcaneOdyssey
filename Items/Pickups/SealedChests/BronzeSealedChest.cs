using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Pickups.SealedChests
{
	public class BronzeSealedChest : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.IsAPickup[Type] = true;
			ItemID.Sets.ItemsThatShouldNotBeInInventory[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.maxStack = AOPlayer.MaxSealedChests;
		}

		public override bool OnPickup(Player player)
		{
			player.ArcaneOdyssey().AddBronzeSealed(Item.stack);
			return false;
		}

		public override bool CanPickup(Player player) => player.ArcaneOdyssey().BronzeSealed < AOPlayer.MaxSealedChests;
	}
}
