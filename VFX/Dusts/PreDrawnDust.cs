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
		internal Rectangle Dimensions;
		public int dustIndex;
		public Vector2 Centre
		{
			get => Main.dust[dustIndex].position + (new Vector2(Texture2D.Height() / MaxFrames, Texture2D.Width()) / 2f);
			set => Main.dust[dustIndex].position = value - (new Vector2(Texture2D.Height() / MaxFrames, Texture2D.Width()) / 2f);
		}

        public override void OnSpawn(Dust dust)
        {
            dustIndex = dust.dustIndex;
        }

		public override void SetStaticDefaults()
		{
			Dimensions = new Rectangle(0, Texture2D.Height() / MaxFrames, Texture2D.Width(), Texture2D.Height());
		}

		public override bool PreDraw(Dust dust)
		{
			Main.EntitySpriteDraw(Texture2D.Value, Centre - Main.screenPosition, Dimensions with { Y = Dimensions.Y * (dust.frame.Y / AOUtils.DefaultDustDimensions.Y.Round())}, dust.color with { A = (byte)(255 - dust.alpha) }, dust.rotation / RotationDivision, Vector2.Zero, dust.scale, SpriteEffects.None);
			return false;
		}
	}
}
