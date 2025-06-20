using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Items.Weapons
{
    public class SunkenSword : AOWeapon
    {
        public override float AOSpeed => 1.2f;
        public override float AOSize => .9f;
        public override float AODamage => 1f;
        public override int AOValue => 900;
        public override int AORarity => AORarities.Rare;
        public override int AOWeaponTier => AOWeaponTiers.Excellent;

        public override AODebuff WeaponDebuff => new AODebuff(BuffID.Wet, 60 * 5);

        public override void SetDefaultsWeapon()
        {
            Item.width = Item.height = 42;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.SplashWeak;
        }

        public override void ModifyHitNPC2(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.dashType != DashID.None && !player.HasBuff<RisenTide>())
            {
                modifiers.ScalingArmorPenetration = AddableFloat.Zero + 1f;
                modifiers.SetCrit();
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DD2SquireBetsySword);
            recipe.AddIngredient<ArcaniumScrap>(2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

