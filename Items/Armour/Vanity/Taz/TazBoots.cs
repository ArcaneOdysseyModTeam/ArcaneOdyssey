using ArcaneOdyssey.Items.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Vanity.Taz
{
	[AutoloadEquip(EquipType.Legs)]
	public class TazBoots : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
		}
	}
}
