using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Items.Debug
{
	public class EliusArenaSpawner : BaseItem
	{
		public override AORarities AORarity => AORarities.Special;

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
		}
	}
}
