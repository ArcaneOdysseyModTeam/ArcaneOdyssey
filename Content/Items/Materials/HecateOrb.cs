using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class HecateOrb : ModItem
    {
        public int AOValue = 10000;
        
        public int AORarity = AORarities.Rare;
        public override void SetDefaults()
        {
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, Item.rare);
            Item.maxStack = 3;
        }
    }
}
