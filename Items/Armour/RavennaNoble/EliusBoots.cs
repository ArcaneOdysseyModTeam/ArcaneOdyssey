using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Legs)]
	public class EliusBoots : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Average;
		public override Rarities Rarity => Rarities.Uncommon;
		public override int Value => 60;
		public override int AOAgility => 11;
		public override int AOPower => 9;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
		}
	}
}
