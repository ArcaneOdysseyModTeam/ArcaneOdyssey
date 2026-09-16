using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Armour.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class HeadlessHeadWithWig : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;
		public override void SetStaticDefaults()
		{
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
			ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 50;
			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<HeadlessHead>().AddIngredient(ItemID.FamiliarWig).Register();
		}

		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			return slot == equipSlotHead;
		}
	}
}
