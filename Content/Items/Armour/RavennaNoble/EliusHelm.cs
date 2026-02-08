using ArcaneOdyssey.Content.Items.Base;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.RavennaNoble
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
		public override AOItemTiers ArmourTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Rare;
		public override int AOValue => 80;


		public override int AODefense => 45;

		public override int AOAgility => 9;
		public override int AOPower => 7;
	}
}
