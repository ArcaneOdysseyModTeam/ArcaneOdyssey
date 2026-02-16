using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.DrawLayers
{
	public class PhoenixWingsLayer : PlayerDrawLayer
	{
		public static Asset<Texture2D> phoenixWingsTex;

		public override void Load()
		{
			phoenixWingsTex = ModContent.Request<Texture2D>(AOUtils.GetTexture<PhoenixMagic>() + $"_{EquipType.Wings}Glow");
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, nameof(PhoenixMagic), EquipType.Wings);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawPlayer.dead)
				return;
			Texture2D texture = phoenixWingsTex.Value;
			Vector2 pos = new((int)(drawInfo.Center.X - Main.screenPosition.X - 6f * drawPlayer.direction), (int)(drawInfo.Center.Y - Main.screenPosition.Y - drawPlayer.height / 2f));
			DrawData d = new(texture, pos, texture.Frame(1, 4, 0, drawInfo.drawPlayer.wingFrame), Color.White, 0f, new(texture.Width / 2, texture.Height / 18), 1f, drawInfo.playerEffect, 0) { shader = drawInfo.drawPlayer.cWings };
			drawInfo.DrawDataCache.Add(d);
		}
	}
}