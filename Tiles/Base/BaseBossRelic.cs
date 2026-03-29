using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Tiles.Base
{
	public abstract class BaseBossRelic : ModTile
	{
		public const int FrameWidth = 18 * 3;
		public const int FrameHeight = 18 * 4;
		public override string Texture => $"{Mod.Name}/Assets/RelicBase";

		public Asset<Texture2D> FloaterTexture;

		public override void Load()
		{
			FloaterTexture = ModContent.Request<Texture2D>(base.Texture, AssetRequestMode.ImmediateLoad);
		}

		public override void Unload()
		{
			FloaterTexture = null;
		}

		public override bool CreateDust(int i, int j, ref int type) => false;

		public override void SetStaticDefaults()
		{
			Main.tileShine[Type] = 400;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleHorizontal = false;

			TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.styleLineSkipVisualOverride = 0;

			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(233, 207, 94), Language.GetText("MapObject.Relic"));
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			tileFrameX %= FrameWidth;
			tileFrameY %= FrameHeight * 2;
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
			{
				Main.instance.TilesRenderer.AddSpecialPoint(i, j, Terraria.GameContent.Drawing.TileDrawing.TileCounterType.CustomNonSolid); // This tile is not Main.tileSolid, so CustomNonSolid is the correct choice here.
			}
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Point p = new(i, j);
			Tile tile = Main.tile[p.X, p.Y];
			if (!tile.HasTile)
			{
				return;
			}


			Vector2 origin = FloaterTexture.Size() / 2f;
			Vector2 worldPos = p.ToWorldCoordinates(24f, 64f);

			Color colour = Lighting.GetColor(p.X, p.Y);

			bool direction = tile.TileFrameY / FrameHeight != 0;
			SpriteEffects mode = direction ? SpriteEffects.FlipHorizontally : SpriteEffects.None;


			float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
			Vector2 drawPos = worldPos - Main.screenPosition + new Vector2(0f, -45f) + new Vector2(0f, offset * 4f);


			spriteBatch.Draw(FloaterTexture.Value, drawPos, null, colour, 0f, origin, 1f, mode, 0f);


			float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) * 0.3f + 0.7f;
			Color effectColor = colour;
			effectColor.A = 0;
			effectColor = effectColor * 0.1f * scale;
			for (float num5 = 0f; num5 < 1f; num5 += 355f / (678f * (float)Math.PI))
			{
				spriteBatch.Draw(FloaterTexture.Value, drawPos + (MathHelper.TwoPi * num5).ToRotationVector2() * (6f + offset * 2f), null, effectColor, 0f, origin, 1f, mode, 0f);
			}
		}
	}

}
