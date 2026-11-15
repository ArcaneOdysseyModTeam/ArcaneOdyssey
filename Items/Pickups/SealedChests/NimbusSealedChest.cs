using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Pickups.SealedChests
{
	public class NimbusSealedChest : BaseItem
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
			player.ArcaneOdyssey().AddNimbusSealed(Item.stack);
			return false;
		}

		public override bool CanPickup(Player player) => player.ArcaneOdyssey().NimbusSealed < AOPlayer.MaxSealedChests;
	}
}
