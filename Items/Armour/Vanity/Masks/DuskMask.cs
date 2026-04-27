using ArcaneOdyssey.Items.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Vanity.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class DuskMask : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
			Item.width = 24;
			Item.height = 26;
		}
	}
}
