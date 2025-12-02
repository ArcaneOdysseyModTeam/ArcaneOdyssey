using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Dusts
{
	public abstract class PreDrawnDust : ModDust
	{
		public abstract int MaxFrames { get; }
		public virtual float RotationDivision => 1f;

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Texture2D.Height() / MaxFrames * Main.rand.Next(MaxFrames), Texture2D.Width(), Texture2D.Height() / MaxFrames);
        }

        public override bool PreDraw(Dust dust)
        {
            Vector2 dimensions = new(dust.frame.Width, dust.frame.Height);
            Main.EntitySpriteDraw(Texture2D.Value, dust.Centre() - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation / RotationDivision, dimensions / 2f, dust.scale, SpriteEffects.None);
            return base.PreDraw(dust);
        }
	}
}
