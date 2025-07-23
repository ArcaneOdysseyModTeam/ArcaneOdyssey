using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Items
{
    public class HecateOrb : ModItem
    {
        public int AOValue = 10000;
        public int AORarity = AORarities.Rare;
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, Item.rare);
        }
    }
}
