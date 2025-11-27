using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
    public class BlizzardPulsar : PulsarSpell
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
    }
}
