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
        public int charge = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            AOPlayer playah = player.AOPlayer();
            if (charge < 3*60 && playah.myCircle is not null && playah.myCircle.ai[0] < 1)
            {
                Projectile.position = playah.myCircle.position;
                charge++;
            }
            else
            {
                player.reuseDelay = 60;
                float dmgmult = charge / 60f;
                // explode here
                Kill();
            }
        }
    }
}
