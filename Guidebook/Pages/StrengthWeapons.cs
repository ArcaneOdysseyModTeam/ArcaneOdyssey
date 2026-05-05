using ArcaneOdyssey.Items.Base;
using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	internal class StrengthWeapons : GuidebookPage
	{
		public override int PageNum => After<AboutGodSouls>();

		public override bool MetConditions(Player player)
		{
			if (player is not null)
			{
				if (player.HasTypeInInventory<Weapon>(e => e.WeaponsType == WeaponType.Strength))
				{
					return true;
				}
				if (player.PlayerItem()?.ArcaneOdyssey() is not null)
				{
					return player.PlayerItem().ArcaneOdyssey().WeaponsType == WeaponType.Strength;
				}
			}
			return false;
		}
	}
}
