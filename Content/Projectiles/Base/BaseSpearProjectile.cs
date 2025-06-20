using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
    public abstract class BaseSpearProjectile : ModProjectile
    {
        public virtual float Speed => 3f;
        public Item? originalItem = null;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = player.itemAnimation;
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter);
            Projectile.position += Projectile.velocity * Projectile.ai[0];
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = Speed;
                Projectile.netUpdate = true;
            }

            if (player.itemAnimation < player.itemAnimationMax / 2)
            {
                Projectile.ai[0] -= Speed/3;
                if (Projectile.localAI[0] == 0f) 
                {
                    Projectile.localAI[0] = 1f;
                    EffectBeforeReelBack();
                }
            }
            
            else
            {
                Projectile.ai[0] += Speed/4;
            }

            if (player.itemAnimation <= 1)
                Projectile.Kill();

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.PiOver4;
            if (Projectile.spriteDirection == -1)
                Projectile.rotation -= MathHelper.PiOver2;
            if (player.itemAnimation == 2)
            {
                Projectile.Kill();
                player.reuseDelay = 2;
            }
        }

        public virtual void EffectBeforeReelBack() { }
    }
}
