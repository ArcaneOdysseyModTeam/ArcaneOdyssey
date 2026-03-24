using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class EvanderCape : Base.Armour
	{
		public override Rarities Rarity => Rarities.Uncommon;
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 170;
		public override int AOPierce => 21;
		public override int AOValue => 75;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}
