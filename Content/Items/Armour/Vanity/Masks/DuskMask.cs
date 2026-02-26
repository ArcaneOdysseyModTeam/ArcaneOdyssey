using ArcaneOdyssey.Content.Items.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.Vanity.Masks
{
	[AutoloadEquip(EquipType.Head)]
	public class DuskMask : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.vanity = true;
			Item.width = 24;
			Item.height = 26;
		}
	}
}
