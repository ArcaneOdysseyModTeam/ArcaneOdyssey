using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
    public class ExplosionTracker : AOPlayerProjectile
    {
        public float charge = 0f;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AOPlayer playah = player.AOPlayer();
            if (charge < 2f && playah.myCircle is not null && playah.myCircle.ai[0] < 1)
            {
                Projectile.position = playah.myCircle.position;
                charge += 1f/60f;
            }
            else
            {
                // explode here
                Kill();
            }
        }
    }
}
