using ArcaneOdyssey.Items.Base;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Head)]
	public class EliusHelm : AOArmour
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

		public override AOItemTiers ArmourTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Rare;
		public override int AOValue => 80;

		public override int AOAgility => 9;
		public override int AOPower => 7;
	}
}
