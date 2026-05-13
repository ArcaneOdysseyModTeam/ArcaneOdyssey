using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Head)]
	public class EliusHelm : Base.Armour
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
		}

		public override ItemTiers ArmourTier => ItemTiers.Average;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override int Value => 80;

		public override short AOAgility => 9;
		public override short AOPower => 7;
	}
}
