using System;
using Terraria.DataStructures;

namespace ArcaneOdyssey.AOPlayers.DrawLayers
{
	public class BlockingIndicator : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new Between();

		public static Asset<Texture2D> indicator = null;

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return Show
			&& !Main.hideUI
			&& drawInfo.drawPlayer.whoAmI == Main.myPlayer
			&& drawInfo.drawPlayer.active
			&& !drawInfo.drawPlayer.DeadOrGhost
			&& drawInfo.shadow == 0
			&& !Main.gameMenu
			&& !drawInfo.drawPlayer.ArcaneOdyssey().Blocking;
		}

		public static bool Show;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Show = false;
			var fade = (MathF.Sin(AOUtils.UpdateCount * 4f) + 2f) / 4f;
			var data = new DrawData(indicator.Value, Main.ScreenSize.ToVector2() / 2f, null, Color.White * fade, -drawInfo.rotation, indicator.Size() / 2f, 3f, SpriteEffects.None);
			drawInfo.DrawDataCache.Add(data);
		}

		public override void Load()
		{
			indicator = AOUtils.Request(AOUtils.BlockTexture, ref indicator);
		}
	}
}
