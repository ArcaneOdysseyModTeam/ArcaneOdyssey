using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Armour.Vanity.Taz
{
	[AutoloadEquip(EquipType.Body)]
	public class TazChest : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
		}
	}
}
