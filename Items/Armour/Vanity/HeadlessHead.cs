using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class HeadlessHead : BaseItem
	{
		public override AORarities AORarity => AORarities.Special;

		public override void SetStaticDefaults()
		{
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 50;
			Item.vanity = true;
		}
		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			return slot == equipSlotHead;
		}
	}
}
