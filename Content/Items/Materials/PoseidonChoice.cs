using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    [LegacyName("HecateOrb")]
    public class PoseidonChoice : ModItem
    {
        public int AOValue = 10000;
        
        public AORarities AORarity = AORarities.Legendary;

        public override void SetDefaults()
        {
            Item.rare = (int)AORarity;
            Item.value = GalleonToCopper(AOValue);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = Type;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
    }
}
