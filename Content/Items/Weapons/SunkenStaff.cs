using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Items.Weapons
{
    public class SunkenStaff : AOWeapon
    {
        public override float AOSpeed => .9f;
        public override float AOSize => 1.25f;
        public override float AODamage => 1f;
        public override int AOValue => 1350;
        public override int AORarity => AORarities.Rare;
        public override int AOWeaponTier => AOWeaponTiers.Excellent;
        public override AODebuff WeaponDebuff => new AODebuff(BuffID.Wet, 60 * 5);

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Spears[Item.type] = true;
        }

        public override void SetDefaultsWeapon()
        {
            Item.shoot = ModContent.ProjectileType<SunkenStaffProjectile>();
            Item.width = Item.height = 40;
            Item.UseSound = SoundID.SplashWeak;
            Item.shootSpeed = 12f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MonkStaffT3);
            recipe.AddIngredient<ArcaniumScrap>(2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
