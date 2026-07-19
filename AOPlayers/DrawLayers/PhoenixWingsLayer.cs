using ArcaneOdyssey.Items.Accessories.Helpers;
using Terraria.DataStructures;

namespace ArcaneOdyssey.AOPlayers.DrawLayers
{
	public class PhoenixWingsLayer : PlayerDrawLayer // PORT use new item equipment draw hooks
	{
		public static Asset<Texture2D> phoenixWingsTex;

		public override void Load()
		{
			phoenixWingsTex = ModContent.Request<Texture2D>(AOUtils.GetTexture<PhoenixWings>() + $"_{EquipType.Wings}Glow");
		}

		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, nameof(PhoenixWings), EquipType.Wings);

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;
			if (player.DeadOrGhost)
				return;
			Texture2D texture = phoenixWingsTex.Value;
			Vector2 Position = drawInfo.Position;
			Vector2 pos = new((int)(Position.X - Main.screenPosition.X + (player.width / 2) - (9 * player.direction)), (int)(Position.Y - Main.screenPosition.Y + (player.height / 2 + player.HeightOffsetVisual / 2f) + 2f * player.gravDir));
			Color color = Color.White * (1 - drawInfo.shadow);
			DrawData d = new(texture, pos, texture.Frame(1, 4, 0, player.wingFrame), color, 0f, new Vector2(texture.Width / 2, texture.Height / 8), 1f, drawInfo.playerEffect, 0)
			{
				shader = player.cWings
			};
			drawInfo.DrawDataCache.Add(d);
		}
	}
}