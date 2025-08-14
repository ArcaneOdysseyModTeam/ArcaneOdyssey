using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class HecateOrb : ModItem
    {
        public int AOValue = 10000;
        public new string LocalizationCategory => "Items.Materials";
        public int AORarity = AORarities.Rare;
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, Item.rare);
        }
    }
}
