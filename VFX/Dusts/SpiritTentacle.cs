using Microsoft.Xna.Framework;
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
            Main.EntitySpriteDraw(Tentacle, dust.Centre()/* - (Tentacle.Size() / 2 * dust.scale)*/ - Main.screenPosition, null, dust.color with { A = (byte)(255 - dust.alpha)}, dust.rotation/3f, Vector2.Zero, dust.scale, SpriteEffects.None);
            return false;
        }
    }
}