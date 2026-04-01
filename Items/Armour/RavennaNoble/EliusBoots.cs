using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Legs)]
	public class EliusBoots : Base.Armour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Average;
		public override Rarities Rarity => Rarities.Uncommon;
		public override int AOValue => 60;
		public override int AOAgility => 11;
		public override int AOPower => 9;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
		}
	}
}
