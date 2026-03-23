using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Legs)]
	public class EliusBoots : Base.Armour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Rare;
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
