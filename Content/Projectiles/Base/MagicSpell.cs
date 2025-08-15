using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
    public abstract class MagicSpell : AOPlayerProjectile
    {
        public virtual void SetDefaultsSpell() { }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            SetDefaultsSpell();
        }

        /// <summary>
        /// does not change when the player's imbue changes, make sure to assign in the ai using ??= to only apply on the first frame
        /// </summary>
        public AOMagic thisMagic = null;
        public override void OnKill(int timeLeft)
        {
            if (aoPlayerOwner is not null)
            {
                if (aoPlayerOwner.imbue is not null)
                {
                    aoPlayerOwner.imbue.KillDust(Projectile.position, Projectile.scale);
                }
            }
        }
    }
}
