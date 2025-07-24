using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
    public class BlastSpell : BaseSpell
    {
        public override void SetDefaultsSpell()
        {
            Projectile.damage = 10;
            Projectile.height = Projectile.width = 64;
            Projectile.scale = .6f;
        }
    }
}
