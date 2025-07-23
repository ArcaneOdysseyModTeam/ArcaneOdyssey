using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
    public abstract class DefaultScroll : ModItem
    {
        public int AOValue = 500;
        public int AORarity = AORarities.Rare;
        public virtual void SetDefaultsScroll() { }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = AORarity;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = GalleonToCopper(AOValue, Item.rare);
            SetDefaultsScroll();
        }
    }
}
