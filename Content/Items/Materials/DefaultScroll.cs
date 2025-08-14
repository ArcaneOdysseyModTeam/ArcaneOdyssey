using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class DefaultScroll : ModItem
    {
        public int AOValue = 500;
        public int AORarity = AORarities.Rare;
        public virtual void SetDefaultsScroll() { }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.noUseGraphic = true;
            Item.rare = AORarity;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.value = GalleonToCopper(AOValue, Item.rare);
            SetDefaultsScroll();
        }

        public override void UpdateInventory(Player player)
        {
            AOPlayer playah = player.GetModPlayer<AOPlayer>();
            if (playah.imbue is not null)
            {
                Item.color = playah.imbue.MagicColour;
            }
            else Item.color = Color.Transparent;
        }

        
    }
}
