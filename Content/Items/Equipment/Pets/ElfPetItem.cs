using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles.MusicBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;
using ArcaneOdyssey.Content.Projectiles.Pets;
using ArcaneOdyssey.Content.Buffs.Pets;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Equipment.Pets
{
    public class ElfPetItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<ElfPetProjectile>();
            Item.buffType = ModContent.BuffType<ElfPetBuff>();
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType,3600,true);
            }
        }
    }
}