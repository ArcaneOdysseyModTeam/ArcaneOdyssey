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

        public override void SetDefaultsWeapon()
        {
            Item.width = Item.height = 64;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.SplashWeak;
        }


        public override bool? UseItem(Player player)
        {
            
            return null;
        }

        public override void AddRecipes()
        {
            
        }
    }
}
