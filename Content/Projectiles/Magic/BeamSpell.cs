using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
    public class BeamSpell : MagicSpell
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.height = Projectile.width = 4; // hitscan
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 75;
        }

        public override bool PreDraw(ref Color lightColor) => false;
    }
}
