using System;
using ArcaneOdyssey.Items.Base;
using Terraria;
using ArcaneOdyssey.Imbues.Magic.Mythical;
using Terraria.ID;

namespace ArcaneOdyssey.Items.Debug
{
	public class MatrixTerminal : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;
		public static int statToMod = 0;
		public static float statMult = 1f;
		public static string[] statsList = {"Size","Speed","Damage"};

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
		public static void RoundMatrixStats()
		{
			MatrixMagic.MatrixDamage = MathF.Round(MatrixMagic.MatrixDamage * 100)/100;
			MatrixMagic.MatrixSpeed = MathF.Round(MatrixMagic.MatrixSpeed * 100)/100;
			MatrixMagic.MatrixSize = MathF.Round(MatrixMagic.MatrixSize * 100)/100;
			MatrixTerminal.statMult = MathF.Round(MatrixTerminal.statMult * 100)/100;
		}
		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				switch (MatrixTerminal.statToMod) {
					case 0:
						MatrixMagic.MatrixSize -= MatrixTerminal.statMult;
						RoundMatrixStats();
						Main.NewText("Size set to: " + MatrixMagic.MatrixSize);
					break;
					case 1:
						MatrixMagic.MatrixSpeed -= MatrixTerminal.statMult;
						RoundMatrixStats();
						Main.NewText("Speed set to: " + MatrixMagic.MatrixSpeed);
					break;
					case 2:
						MatrixMagic.MatrixDamage -= MatrixTerminal.statMult;
						RoundMatrixStats();
						Main.NewText("Damage set to: " + MatrixMagic.MatrixDamage);
					break;
				}
			} else
			{
				switch (MatrixTerminal.statToMod) {
					case 0:
						MatrixMagic.MatrixSize += MatrixTerminal.statMult;
						RoundMatrixStats();
						Main.NewText("Size set to: " + MatrixMagic.MatrixSize);
					break;
					case 1:
						MatrixMagic.MatrixSpeed += MatrixTerminal.statMult;
						RoundMatrixStats();
						Main.NewText("Speed set to: " + MatrixMagic.MatrixSpeed);
					break;
					case 2:
						MatrixMagic.MatrixDamage += MatrixTerminal.statMult;
						RoundMatrixStats();
						Main.NewText("Damage set to: " + MatrixMagic.MatrixDamage);
					break;
				}
				
			}
			return true;
		}
	}
}
