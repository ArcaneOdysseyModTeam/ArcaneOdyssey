using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers.DrawLayers.SpellSlotSystem
{
	public class CurrentAttackDisplay : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;

		public static Asset<Texture2D> backgroundSprite;
		public override void Load()
		{
			backgroundSprite = Mod.Assets.Request<Texture2D>("Assets/GelBuffBackground");
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow == 0f)
			{
				Player player = drawInfo.drawPlayer;
				if (player.DeadOrGhost)
					return;

				if (Main.gameMenu)
					return;

				if (player.PlayerItem().ModItem is Imbuable imbue)
				{

				}

				var pos = drawInfo.Position;
				Texture2D texture = backgroundSprite.Value;
				DrawData d = new(texture, pos, texture.Frame(), Color.White, 0f, texture.Size()/2f, 1f, SpriteEffects.None, 0);
				drawInfo.DrawDataCache.Add(d);
			}
		}
	}
}
