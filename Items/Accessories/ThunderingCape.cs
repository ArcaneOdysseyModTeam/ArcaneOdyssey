using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class ThunderingCape : BaseItem
	{
		public override Rarities Rarity => Rarities.Mystic;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.expert = true;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.ArcaneOdyssey().thundering = Item;
			player.ArcaneOdyssey().hiddenThunder = hideVisual;
		}
	}
}
