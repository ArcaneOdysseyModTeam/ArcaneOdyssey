using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
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

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaultsWeapon()
        {
            Item.width = Item.height = 42;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.SplashWeak;
        }

        public override bool AltFunctionUse(Player player)
        {
            return !player.HasBuff<RisenTide>();
        }

        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2 && !player.HasBuff<RisenTide>())
            {
                player.AddBuff(ModContent.BuffType<RisenTide>(), (int)(60 * (CurrentImbue is not null ? CurrentImbue.AOImbueSpeed : 1) * AOSpeed * 5));
                SoundEngine.PlaySound(SoundID.Splash, player.position);
                player.velocity.Y -= 20;
                // Adds dust
                Dust.NewDust(player.position,3,3,DustID.DungeonWater,0f,-10f,255,new Color(null),1f);
            }
            return null;
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

