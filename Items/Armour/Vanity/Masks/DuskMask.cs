using ArcaneOdyssey.Items.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Vanity.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class DuskMask : BaseItem
	{
		public override Rarities Rarity => Rarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
			Item.width = 24;
			Item.height = 26;
		}
	}
}
