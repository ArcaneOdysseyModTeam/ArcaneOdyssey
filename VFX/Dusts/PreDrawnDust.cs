using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Dusts
{
	public abstract class PreDrawnDust : ModDust
	{
        public virtual int Rows => 1;
        public virtual int Columns => 1;
        public virtual float RotationDivision => 1f;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(Texture2D.Width() / Columns * Main.rand.Next(Columns), Texture2D.Height() / Rows * Main.rand.Next(Rows), Texture2D.Width() / Columns, Texture2D.Height() / Rows);
        }

        public override bool PreDraw(Dust dust)
        {
            Vector2 dimensions = new(dust.frame.Width, dust.frame.Height);
            Main.EntitySpriteDraw(Texture2D.Value, dust.Centre() - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation / RotationDivision, dimensions / 2f, dust.scale, SpriteEffects.None);
            return false;
        }
	}
}
