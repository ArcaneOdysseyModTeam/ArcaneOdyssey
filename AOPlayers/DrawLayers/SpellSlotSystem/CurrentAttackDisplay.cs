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

				if (player.whoAmI != Main.myPlayer)
					return;

				if (player.PlayerItem().ModItem is Imbuable imbue)
				{
					Vector2 offset;
					if (ExternalModSupport.HasFargos)
					{
						offset = new Vector2(Player.defaultWidth / 2f, -64);
					}
					else
					{
						offset = new Vector2(Player.defaultWidth / 2f, -32);
					}
					var pos = drawInfo.Position + offset;
					Texture2D texture = backgroundSprite.Value;

					DrawData a = new(texture, pos - Main.screenPosition, texture.Frame(), Color.White, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					DrawData b = new(texture, pos - new Vector2(32, -3) - Main.screenPosition, texture.Frame(), Color.White * .75f, 0f, texture.Size() / 2f, .75f, SpriteEffects.None, 0);

					DrawData c = new(texture, pos - new Vector2(-32, -3) - Main.screenPosition, texture.Frame(), Color.White * .75f, 0f, texture.Size() / 2f, .75f, SpriteEffects.None, 0);

					var spell = imbue.selectedAttack;

					drawInfo.DrawDataCache.AddRange(a, b, c);
				}
			}
		}
	}
}
