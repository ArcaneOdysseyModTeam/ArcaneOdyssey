using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.VFX.Dusts
{
	public abstract class PreDrawnDust : ModDust
	{
		public abstract int MaxFrames { get; }
        public virtual float RotationDivision => 1f;
        private Rectangle Dimensions;

        public override void SetStaticDefaults()
        {
            Dimensions = new Rectangle(0, Texture2D.Height() / MaxFrames, Texture2D.Width(), Texture2D.Height());
        }

		public override bool PreDraw(Dust dust)
		{
            Main.EntitySpriteDraw(Texture2D.Value, dust.Centre() - Main.screenPosition, Dimensions with { Y = Dimensions.Y * (dust.frame.Y / 8)}, dust.color with { A = (byte)(255 - dust.alpha) }, dust.rotation / RotationDivision, Vector2.Zero, dust.scale, SpriteEffects.None);
			return false;
		}
	}
}
