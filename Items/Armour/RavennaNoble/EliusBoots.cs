using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Legs)]
	public class EliusBoots : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Average;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override int Value => 60;
		public override short AOAgility => 11;
		public override short AOPower => 9;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
		}
	}
}
