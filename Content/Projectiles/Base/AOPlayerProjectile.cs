using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
    /// <summary>
    /// Projectile created by the player, usually via weapon
    /// </summary>
    public abstract class AOPlayerProjectile : ModProjectile
    {
        public Item? originalItem = null;
        public AOPlayer? aoPlayerOwner = null;

        public const float AOSpeed = 1f;
        public const float AOSize = 1f;
        public const float AODamage = 1f;


        // Projectile.ai[0] is 
        // Projectile.ai[1] is 
        // Projectile.ai[2] is 

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (aoPlayerOwner is not null)
            if (aoPlayerOwner.imbue is not null)
            {
                modifiers.FinalDamage *= aoPlayerOwner.imbue.AOImbueDamage;
            }
        }
    }
}
