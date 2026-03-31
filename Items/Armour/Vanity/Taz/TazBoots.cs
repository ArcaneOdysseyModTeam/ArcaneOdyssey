using ArcaneOdyssey.Items.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Vanity.Taz
{
	[AutoloadEquip(EquipType.Legs)]
	public class TazBoots : BaseItem
	{
		public override Rarities Rarity => Rarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
		}
	}
}
