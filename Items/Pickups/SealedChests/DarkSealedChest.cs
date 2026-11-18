using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Pickups.SealedChests
{
	public class DarkSealedChest : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Mystic;
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
			player.ArcaneOdyssey().AddDarkSealed(Item.stack);
			return false;
		}

		public override bool CanPickup(Player player) => player.ArcaneOdyssey().DarkSealed < AOPlayer.MaxSealedChests;
	}
}
