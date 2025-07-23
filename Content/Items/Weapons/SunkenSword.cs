using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;

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
                player.velocity.Y *= 0.1f;
                player.velocity.Y -= 20;
                // Adds dust
                for(int dustCountInt = 0;dustCountInt<50;dustCountInt++){
                    Dust.NewDust(player.position+new Vector2(-20f+(40f*(float)(System.Math.Sin((double)dustCountInt*3.0))),0f),3,3,DustID.Water,player.velocity.X*(float)dustCountInt*0.02f,-1f * (float)dustCountInt,255,new Color(255,255,255,255),1.3f);
                    Dust.NewDust(player.position+new Vector2(20f+(40f*(float)(System.Math.Sin(((double)dustCountInt*3.0)+(3.14)))),0f),3,3,DustID.Water,player.velocity.X*(float)dustCountInt*0.02f,-1f * (float)dustCountInt,255,new Color(255,255,255,255),1.3f);
                    Dust.NewDust(player.position+new Vector2(-20f+(40f*(float)(System.Math.Sin((double)dustCountInt*3.0))),0f),3,3,DustID.DungeonWater,player.velocity.X*(float)dustCountInt*0.02f,-0.5f * (float)dustCountInt,255,new Color(255,255,255,255),1f);
                    Dust.NewDust(player.position+new Vector2(20f+(40f*(float)(System.Math.Sin(((double)dustCountInt*3.0)+(3.14)))),0f),3,3,DustID.DungeonWater,player.velocity.X*(float)dustCountInt*0.02f,-0.5f * (float)dustCountInt,255,new Color(255,255,255,255),1f);
                }
                //Rising tide text
                CombatText.NewText(player.Hitbox, new Color(0,105,255,255), Mod.GetLocalization("PopupText.RisingTide").Value);
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

