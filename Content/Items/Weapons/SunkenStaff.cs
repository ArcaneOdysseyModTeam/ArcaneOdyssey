using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.CodeAnalysis.Operations;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using ArcaneOdyssey.Content.Items.Materials;

namespace ArcaneOdyssey.Content.Items.Weapons
{
    public class SunkenStaff : AOWeapon
    {
        public override bool? ColdWeapon => true;
        public override float AOSpeed => .9f;
        public override float AOSize => 1.25f;
        public override float AODamage => 1f;
        public override int AOValue => 1350;
        public override AORarities AORarity => AORarities.Rare;
        public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Excellent;
        public override AODebuffRequirement WeaponDebuff => new AODebuffRequirement(BuffID.Wet, 600);


        public override void SetDefaultsWeapon()
        {
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ModContent.ProjectileType<SunkenStaffProjectile>();
            Item.width = Item.height = 40;
            Item.channel = true;
            Item.UseSound = SoundID.SplashWeak;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;
            Item.useAnimation = Item.useTime = 25;
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
