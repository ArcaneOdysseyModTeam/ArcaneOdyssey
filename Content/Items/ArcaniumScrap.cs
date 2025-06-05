using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Items
{
    public class ArcaniumScrap : ModItem
    {
        public int AOValue = 400;
        public int AORarity = AORarities.Rare;
        public override void SetDefaults()
        {
            Item.width = 35;
            Item.height = 30;
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, Item.rare);
        }
    }
}
