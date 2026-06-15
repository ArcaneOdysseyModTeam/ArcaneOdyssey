using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Mythical;
using ArcaneOdyssey.Items.Base;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Debug
{
	public class MatrixCopier : BaseItem
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

		private static int index = 0;
		private static Imbuable[] allMagics = [];

		public override bool AltFunctionUse(Player player) => true;

		public override void UseAnimation(Player player)
		{
			var magic = allMagics[index];
			if (player.AltUse())
			{
				MatrixMagic.MatrixCombos = magic.CombinedDebuffs;
				MatrixMagic.MatrixDebuffs = magic.ImbueDebuffs;
				MatrixMagic.MatrixEffects = magic.Effects;
				MatrixMagic.MatrixGimmick = magic.Gimmick;
				MatrixMagic.MatrixDamage = magic.ScrollDamage;
				MatrixMagic.MatrixSize = magic.ScrollSize;
				MatrixMagic.MatrixSpeed = magic.ScrollSpeed;
				Main.NewText($"Copying {magic.Name}!");
			}
			else
			{
				MatrixMagic.MatrixCombos = magic.CombinedDebuffs;
				MatrixMagic.MatrixDebuffs = magic.ImbueDebuffs;
				MatrixMagic.MatrixEffects = magic.Effects;
				MatrixMagic.MatrixGimmick = magic.Gimmick;
				Main.NewText($"Copying synergies, debuffs, and gimmicks of {magic.Name}!");
			}

			if (++index >= allMagics.Length)
			{
				index = 0;
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			allMagics = ModContent.GetContent<Imbuable>().ToArray();
		}
	}
}
