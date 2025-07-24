using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Scrolls
{
    public class BlastScroll : DefaultScroll
    {
        public override void SetDefaultsScroll()
        {
            Item.useTime = 15;
            Item.useAnimation = 60;
            Item.damage = 10;
            Item.shootSpeed = 20;
            Item.shoot = ModContent.ProjectileType<BlastSpell>();
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<AOPlayer>().imbue is not null;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
