using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
    public abstract class AOBaseProjectile : ModProjectile
    {
        public virtual float BaseScale => 1f;
    }
}
