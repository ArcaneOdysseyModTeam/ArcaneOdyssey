using ArcaneOdyssey.Items.Base;
using Terraria;
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
			else if (Main.myPlayer == player.whoAmI)
			{
				Main.NewText("Doesn't work in multiplayer idiot");
			}
			Item.SetDefaults(ItemID.DirtBlock);
		}

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode;
	}
}
