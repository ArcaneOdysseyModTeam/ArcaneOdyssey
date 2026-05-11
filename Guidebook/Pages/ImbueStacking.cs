using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class ImbueStacking : GuidebookPage
	{
		public override int PageNum => After<AboutGodSouls>();

		public override bool MetConditions(Player player)
		{
			if (player.HasTypeInInventory<FightingStyle>() && player.HasTypeInInventory<Imbuable>(e => e is not FightingStyle))
			{
				return Main.hardMode;
			}

			if (player.HasTypeInInventory<SpiritEnergy>() && player.HasTypeInInventory<MagicType>())
			{
				return Main.hardMode;
			}

			return false;
		}
	}
}
