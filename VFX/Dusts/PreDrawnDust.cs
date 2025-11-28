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

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, Texture2D.Height() / MaxFrames * Main.rand.Next(MaxFrames), Texture2D.Width(), Texture2D.Height() / MaxFrames);
            dust.rotation /= RotationDivision;
        }
	}
}
