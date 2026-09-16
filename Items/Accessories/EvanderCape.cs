using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class EvanderCape : BaseArmour
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers ArmourTier => ItemTiers.Good;
		public override ushort AODefense => 170;
		public override short AOPierce => 21;
		public override int Value => 75;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}
