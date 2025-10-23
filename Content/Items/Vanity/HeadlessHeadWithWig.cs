using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class HeadlessHeadWithWig : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Special;
		public override void SetStaticDefaults()
		{
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
			ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = true;
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 50;
			Item.accessory = true;
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
