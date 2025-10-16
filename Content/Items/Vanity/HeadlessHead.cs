using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class HeadlessHead : ModItem
    {
        public override void SetStaticDefaults()
        {
            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
        }

        public override void SetDefaults()
        {
            Item.width = Item.height = 50;
            Item.accessory = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }


    }
}
