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
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles
{
    public class DeathCurse : ModProjectile
    {
        public override void SetDefaults()
        {

            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.width = Projectile.height = 64;
        }
        public override void AI()
        {
            Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, default, 2f)];
            spawnedDust.noGravity = true;
            Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 3f, 0, default, 2.6f)];
            spawnedDust2.noGravity = true;
            if (Projectile.position.Y < 0 || Projectile.velocity.Y > -1)
            {
                Projectile.Kill();
            }
            Projectile.velocity *= 0.999f;
        }
    }
}