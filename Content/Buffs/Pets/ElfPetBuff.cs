using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ArcaneOdyssey.Content.Projectiles.Pets;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Buffs.Pets
{
    public class ElfPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<AOPlayer>().elfPet = true;
            bool projectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<ElfPetProjectile>()] <= 0;
            if (projectileNotSpawned && player.whoAmI == Main.myPlayer) {
				Projectile.NewProjectile(player.GetSource_FromThis(),player.Center,Microsoft.Xna.Framework.Vector2.Zero,ModContent.ProjectileType<ElfPetProjectile>(),0,0f,player.whoAmI);
			}
        }
    }
}