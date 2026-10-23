using ArcaneOdyssey.Items.Blocks.Tiles.Bronze;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.Bronze
{
	public class BronzeBrazierTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style4x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.LavaDeath = false;
			AnimationFrameHeight = TileObjectData.newTile.CoordinateFullHeight;
			TileObjectData.addTile(Type);
			RegisterItemDrop(ModContent.ItemType<BronzeBrazier>());

			AddMapEntry(Color.Brown, ModContent.GetInstance<BronzeBrazier>().DisplayName);
		}

		public const int YFrames = 4;

		public static Asset<Texture2D> GlowMask;
		public string GlowTexture => Texture + "_Glow";

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (AOUtils.RequestIfExists(GlowTexture, ref GlowMask))
			{
				var tile = AOUtils.GetTile(i, j);
				var frame = new Rectangle(tile.TileFrameX, tile.TileFrameY + (AnimationFrameHeight * Main.tileFrame[Type]), 16, 16);
				var drawPos = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) + (16f * new Vector2(i, j)) - Main.screenPosition;
				spriteBatch.Draw(GlowMask.Value, drawPos, frame, Color.White);
			}
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			type = DustID.Torch;
			return true;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 165 / 255f;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (++frameCounter >= 8)
			{
				frameCounter = 0;
				frame = ++frame % YFrames;
			}
		}
	}
}
