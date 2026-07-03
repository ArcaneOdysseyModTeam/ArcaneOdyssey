using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.ArcaneOdysseyMod;

namespace ArcaneOdyssey.Items.Debug
{
	public class EliusArenaSpawner : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;

		public override string Texture => ModContent.GetInstance<EliusArena>().BestiaryIcon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.width = Item.height = 30;
		}

		public override void UseAnimation(Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				WorldGenStuff.SpawnEliusArena();
				Item.TurnToAir();
			}
			else if (Main.netMode == NetmodeID.MultiplayerClient && player.whoAmI == Main.myPlayer)
			{
				if (EliusArenaLoader.eliusArena == default)
				{
					var packet = Mod.GetPacket();
					packet.Write(PacketID.SpawnEliusArena);
					packet.Send();
				}
				Item.TurnToAir();
			}
		}

		public override bool CanUseItem(Player player) => ExternalModSupport.NotInSubworld;
	}
}
