using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.NPCs.Bosses;

namespace ArcaneOdyssey.Items.Pickups
{
	public class ArgosKey : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override string Texture => AOUtils.TerrariaItemTexture(ItemID.GoldenKey);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
			ItemID.Sets.ItemsThatShouldNotBeInInventory[Type] = true;
		}

		public override bool OnPickup(Player player)
		{
			ArgoSpawningSystem.hasArgosKey = true;
			Main.NewText(Tooltip);
			if (!AOUtils.Singleplayer)
				Mod.NewPacket(ArgosKeyPickupSystem.Packet).Send();
			return false;
		}
	}

	public class ArgosKeyPickupSystem : ModSystem
	{
		public static byte Packet;
		public override void Load()
		{
			ArcaneOdysseyMod.HandleExtraPacket += PickupKey;
			Packet = ArcaneOdysseyMod.PacketID.ExtraHandle++;
		}

		private static void PickupKey(BinaryReader reader, int whoAmI, byte command)
		{
			if (command == Packet)
			{
				ArgoSpawningSystem.hasArgosKey = true;
				if (Main.dedServ)
					ArcaneOdysseyMod.Instance.NewPacket(Packet).Send(ignoreClient: whoAmI);
				else
					Main.NewText(ModContent.GetInstance<ArgosKey>().Tooltip);
			}
		}

		public override void Unload()
		{
			ArcaneOdysseyMod.HandleExtraPacket -= PickupKey;
			ArcaneOdysseyMod.PacketID.ExtraHandle--;
		}
	}
}
