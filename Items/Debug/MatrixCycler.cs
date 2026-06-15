using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ID;
using ArcaneOdyssey.Imbues.Magic.Mythical;

namespace ArcaneOdyssey.Items.Debug
{
	public class MatrixCycler : BaseItem
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

		public override bool AltFunctionUse(Player player) => true;

		public override void UseAnimation(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				MatrixTerminal.statMult = 1f;
				MatrixMagic.MatrixDamage = 1f;
				MatrixMagic.MatrixSpeed = 1f;
				MatrixMagic.MatrixSize = 1f;
				MatrixMagic.MatrixDebuffs = [];
				MatrixMagic.MatrixEffects = new();
				MatrixMagic.MatrixCombos = [];
				Main.NewText("Matrix reset");
			}
			else
			{
				MatrixTerminal.statToMod++;
				if (MatrixTerminal.statToMod > 2)
				{
					MatrixTerminal.statToMod = 0;
				}
				Main.NewText("Stat to mod set to: " + MatrixTerminal.statsList[MatrixTerminal.statToMod]);
			}
		}
	}
}
