using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost
{
    public class DarknessBlast : BlastSpell
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void PostDraw(Color lightColor)
        {
            // pulse effect goes here, add to cannon and pulsar too
        }
    }
}
