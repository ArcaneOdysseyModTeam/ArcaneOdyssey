using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Accessories
{
	[AutoloadEquip(EquipType.Back)]
	public class EvanderCape : Base.Armour
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers ArmourTier => ItemTiers.Good;
		public override int AODefense => 170;
		public override int AOPierce => 21;
		public override int Value => 75;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}
