using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Dusts
{
	public abstract class PreDrawnDust : ModDust
	{
		public virtual int Rows => 1;
		public virtual int Columns => 1;

		public override void OnSpawn(Dust dust)
		{
			if (dust.color == default)
				dust.color = Color.White;
			dust.frame = new Rectangle(Texture2D.Width() / Columns * Main.rand.Next(Columns), Texture2D.Height() / Rows * Main.rand.Next(Rows), Texture2D.Width() / Columns, Texture2D.Height() / Rows);
			dust.Centre(dust.position);
			dust.rotation = MathHelper.PiOver2 * Main.rand.Next(4);
			dust.noGravity = true;
		}

		public override bool PreDraw(Dust dust)
		{
			Vector2 dimensions = new(dust.frame.Width, dust.frame.Height);
			Main.EntitySpriteDraw(Texture2D.Value, dust.Centre() - Main.screenPosition, dust.frame, dust.GetAlpha(dust.color), dust.rotation, dimensions / 2f, dust.scale, SpriteEffects.None);
			return false;
		}
	}
}
