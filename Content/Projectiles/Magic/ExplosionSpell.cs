using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
    public class ExplosionSpell : MagicSpell
    {
        // ai[0] will be damage multiplier
        public override void SetDefaults()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.height = Projectile.width = 200;
            Projectile.scale = 1f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 30;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            if (Projectile.TryGetImbue(Main.player[Projectile.owner], out AOMagic imbue))
            {
                imbue.ExplosionEffects(Projectile);
                Projectile.height = Projectile.width = (int)(imbue.AOMagicSize*200);
            }
        }
    }
}
