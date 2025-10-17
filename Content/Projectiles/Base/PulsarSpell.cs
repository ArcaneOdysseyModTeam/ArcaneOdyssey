using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
    public abstract class PulsarSpell : MagicSpell
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.height = Projectile.width = 64;
            Projectile.velocity /= 4;
        }

        public override void AI()
        {
            if (Main.myPlayer == Projectile.owner && ++Projectile.localAI[0] > 30)
            {
                Projectile.localAI[0] = 0;
                var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<ExplosionSpell>(), 40, 0f, Projectile.owner, 1.3f);
                proj.Center = Projectile.Center + Projectile.velocity;
            }
            if (Projectile.frameCounter > 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            Projectile.frameCounter++;
            if (Projectile.ai[2] == 0f)
            {
                Projectile.ai[2] = 1f;
                Projectile.netUpdate = true;
            }
            aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
            Rotate();
            if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
            {
                Kill();
                return;
            }
        }

        public virtual void Rotate()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}
