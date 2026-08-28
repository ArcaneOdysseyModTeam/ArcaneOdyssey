using ArcaneOdyssey.Items.Weapons;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace ArcaneOdyssey.AOPlayers.DrawLayers
{
	public class MusketBarDrawLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new Between();

		public static (Asset<Texture2D>, Asset<Texture2D>) backgroundSprites;
		public override void Load()
		{
			backgroundSprites = (Mod.Assets.Request<Texture2D>("Assets/BayonetBarEmpty"), Mod.Assets.Request<Texture2D>("Assets/BayonetBarFull"));
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return !Main.hideUI
			&& drawInfo.drawPlayer.whoAmI == Main.myPlayer
			&& drawInfo.drawPlayer.active
			&& !drawInfo.drawPlayer.DeadOrGhost
			&& drawInfo.shadow == 0
			&& !Main.gameMenu;
		}

		private float scaler;
		private bool looper;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;
			List<DrawData> drawDatas = [];
			if (player.PlayerItem().ModItem is GildedMusket weapon)
			{
				var barvalue = Main.LocalPlayer.GetModPlayer<GildedPlayer>().BarProgress;
				Vector2 drawPos;
				float rotation;
				float scale = 1f;
				if (looper)
				{
					if (scaler < 1.1f)
					{
						scaler += 1 / 30f;
					}
					else
					{
						looper = false;
					}
				}
				else
				{
					if (scaler > .9f)
					{
						scaler -= 1 / 30f;
					}
					else
					{
						looper = true;
					}
				}
				if (barvalue >= 1f)
				{
					scale = scaler;
				}
				SpriteEffects effects;
				if (player.gravDir > 0)
				{
					drawPos = player.Bottom;
					effects = SpriteEffects.None;
					rotation = 0;
				}
				else
				{
					drawPos = player.Top;
					effects = SpriteEffects.FlipHorizontally;
					rotation = MathHelper.Pi;
				}

				rotation -= drawInfo.rotation;
				drawPos.Y += 32f * player.gravDir;

				drawPos -= player.MountedCenter;
				drawPos = drawPos.RotatedBy(-drawInfo.rotation);
				drawPos += player.MountedCenter;
				drawPos += Vector2.UnitY * player.gfxOffY;

				drawDatas.Add(new(backgroundSprites.Item1.Value, drawPos - Main.screenPosition, null, Color.White, rotation, backgroundSprites.Item1.Size() / 2f, scale, effects));
				drawDatas.Add(new(backgroundSprites.Item2.Value, drawPos - Main.screenPosition, new Rectangle(0, 0, (backgroundSprites.Item2.Width() * barvalue).Round(), backgroundSprites.Item2.Height()), Color.White, rotation, backgroundSprites.Item2.Size() / 2f, scale, effects));
			}
			drawInfo.DrawDataCache.AddRange(drawDatas);
		}

		public override bool IsHeadLayer => false;
	}
}
