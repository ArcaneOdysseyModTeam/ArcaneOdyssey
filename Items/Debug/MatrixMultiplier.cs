using System.Security;
using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Debug
{
	public class MatrixMultiplier : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;
		

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.width = Item.height = 30;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				MatrixTerminal.statMult /= 10;
				if(MatrixTerminal.statMult < 0.01f)
				{
					MatrixTerminal.statMult = 0.01f;
				}
				MatrixTerminal.RoundMatrixStats();
				Main.NewText("Stat mult set to: " + MatrixTerminal.statMult);
			} else
			{
				MatrixTerminal.statMult *= 10;
				if(MatrixTerminal.statMult > 10f)
				{
					MatrixTerminal.statMult = 10;
				}
				MatrixTerminal.RoundMatrixStats();
				Main.NewText("Stat mult set to: " + MatrixTerminal.statMult);
			}
			return true;
		}
	}
}
