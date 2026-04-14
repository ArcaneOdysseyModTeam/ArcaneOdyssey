using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Debug
{
	public class EliusArenaSpawner : BaseItem
	{
		public override Rarities Rarity => Rarities.Special;

		public override string Texture => AOUtils.GelTexture;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.HoldUp;
		}

		public override void UseAnimation(Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				WorldGenStuff.SpawnEliusArena();
			}
			else if (Main.dedServ)
			{
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization("Debug.Attempt").ToNetworkText(), Color.White);
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization("Debug.AnyString", EliusArenaLoader.eliusArena.ToString()).ToNetworkText(), Color.White);
			}
			else
			{
				Main.NewText("Location found on your client:");
				Main.NewText(EliusArenaLoader.eliusArena.ToString());
			}
		}
	}
}
