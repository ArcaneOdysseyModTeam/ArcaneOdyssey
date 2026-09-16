using System;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Imbues.Magic.Mythical;

namespace ArcaneOdyssey.Items.Debug
{
	public class MatrixTerminal : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;
		public static int statToMod = 0;
		public static float statMult = 1f;
		public static string[] statsList = { "Size", "Speed", "Damage" };

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.width = Item.height = 30;
		}

		public override bool AltFunctionUse(Player player) => true;

		public static void RoundMatrixStats()
		{
			MatrixMagic.MatrixDamage = MathF.Round(MatrixMagic.MatrixDamage, 2);
			MatrixMagic.MatrixSpeed = MathF.Round(MatrixMagic.MatrixSpeed, 2);
			MatrixMagic.MatrixSize = MathF.Round(MatrixMagic.MatrixSize, 2);
			statMult = MathF.Round(statMult, 2);
		}

		public override void UseAnimation(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				switch (statToMod)
				{
					case 0:
						MatrixMagic.MatrixSize -= statMult;
						RoundMatrixStats();
						Main.NewText("Size set to: " + MatrixMagic.MatrixSize);
						break;
					case 1:
						MatrixMagic.MatrixSpeed -= statMult;
						RoundMatrixStats();
						Main.NewText("Speed set to: " + MatrixMagic.MatrixSpeed);
						break;
					case 2:
						MatrixMagic.MatrixDamage -= statMult;
						RoundMatrixStats();
						Main.NewText("Damage set to: " + MatrixMagic.MatrixDamage);
						break;
				}
			}
			else
			{
				switch (statToMod)
				{
					case 0:
						MatrixMagic.MatrixSize += statMult;
						RoundMatrixStats();
						Main.NewText("Size set to: " + MatrixMagic.MatrixSize);
						break;
					case 1:
						MatrixMagic.MatrixSpeed += statMult;
						RoundMatrixStats();
						Main.NewText("Speed set to: " + MatrixMagic.MatrixSpeed);
						break;
					case 2:
						MatrixMagic.MatrixDamage += statMult;
						RoundMatrixStats();
						Main.NewText("Damage set to: " + MatrixMagic.MatrixDamage);
						break;
				}

			}
		}
	}
}
