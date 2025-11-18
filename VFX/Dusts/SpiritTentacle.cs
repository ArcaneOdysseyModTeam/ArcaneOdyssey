using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Dusts
{
    public class SpiritTentacle : ModDust
    {
        public Texture2D Tentacle => Mod.Assets.Request<Texture2D>("Assets/SpiritTentacle").Value;
        public override bool PreDraw(Dust dust)
        {
            Main.EntitySpriteDraw(Tentacle, (dust.position + AOUtils.DefaultDustDimensions) - (Tentacle.Size() / 2), null, dust.color with { A = (byte)(255 - dust.alpha) }, dust.rotation, Tentacle.Size() / 2, dust.scale, SpriteEffects.None);
            return false;
        }
    }
}