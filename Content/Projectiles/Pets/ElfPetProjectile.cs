using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Terraria.ModLoader;
namespace ArcaneOdyssey.Content.Projectiles.Pets
{
    public class ElfPetProjectile : ModProjectile
    {
        private Vector2 targetPosition;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AOPlayer modPlayer = player.GetModPlayer<AOPlayer>();
            if (modPlayer.elfPet)
            {
                Projectile.timeLeft = 2;
            }
            targetPosition = player.position;
            Projectile.position = targetPosition;
        }
    }
}