using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Armour.Vanity.Taz
{
	[AutoloadEquip(EquipType.Head)]
	public class TazHat : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
		}
	}
}
