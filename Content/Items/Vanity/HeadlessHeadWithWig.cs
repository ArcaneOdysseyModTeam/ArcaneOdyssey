using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class HeadlessHeadWithWig : ModItem
    {
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
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<HeadlessHead>().AddIngredient(ItemID.FamiliarWig).Register();
        }
    }
}
