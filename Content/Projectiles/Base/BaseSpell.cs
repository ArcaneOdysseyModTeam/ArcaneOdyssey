using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
    public abstract class BaseSpell : AOPlayerProjectile
    {
        public virtual void SetDefaultsSpell() { }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            SetDefaultsSpell();
        }
    }
}
