using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers.DrawLayers
{
	public class ILegLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new Between(PlayerDrawLayers.Shoes, null);

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => !drawInfo.isSitting && drawInfo.drawPlayer.PlayerItem()?.ModItem is IronLeg;

		public static Asset<Texture2D> sprite;
		public override void Load()
		{
			sprite = ModContent.Request<Texture2D>(AOUtils.GetTexture<IronLeg>() + "_Leg");
		}

		protected override void Draw(ref PlayerDrawSet drawinfo)
		{
			DrawData item = new(sprite.Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (drawinfo.drawPlayer.legFrame.Width / 2) + (drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, Color.White * .6f * (1f - drawinfo.shadow), drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}
}
