using ArcaneOdyssey.Imbues.Base;
using Fargowiltas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace ArcaneOdyssey.AOPlayers.DrawLayers
{
	public class ImbueSkillsDisplay : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new Between();

		public static (Asset<Texture2D>, Asset<Texture2D>) backgroundSprites;
		public static Rectangle dimensions;
		public static float spriteSize;
		public override void Load()
		{
			backgroundSprites = (Mod.Assets.Request<Texture2D>("Assets/SelectedScrollIcon"), Mod.Assets.Request<Texture2D>("Assets/UnsellectedScrollIcon"));
		}

		public override void SetStaticDefaults()
		{
			dimensions = backgroundSprites.Item1.Frame();
			spriteSize = dimensions.Width - 4f;
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

		[JITWhenModsEnabled("Fargowiltas")]
		public static bool FargosBuffDisplayActive(PlayerDrawSet drawInfo) => ModContent.GetInstance<FargoPlayerBuffDrawLayer>().GetDefaultVisibility(drawInfo);

		public static readonly int[] FargosIgnoredDebuffs = [
			BuffID.Campfire,
			BuffID.HeartLamp,
			BuffID.Sunflower,
			BuffID.PeaceCandle,
			BuffID.StarInBottle,
			BuffID.Tipsy,
			BuffID.MonsterBanner,
			BuffID.Werewolf,
			BuffID.Merfolk,
			BuffID.CatBast,
			BuffID.BrainOfConfusionBuff,
			BuffID.NeutralHunger
		];

		public override bool IsHeadLayer => false;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;

			if (player.PlayerItem().ModItem is Imbuable imbue)
			{
				List<DrawData> drawDatas = [];

				int yOffset = dimensions.Height;
				if (ExternalModSupport.HasFargos)
				{
					yOffset += (player.buffType.Where(d => Main.debuff[d]).Except(FargosIgnoredDebuffs).Count() / 10) * 32;
				}

				Vector2 drawPos = (player.gravDir > 0 ? player.Top : player.Bottom);
				drawPos.Y -= (32f + yOffset) * player.gravDir;

				drawPos -= player.MountedCenter;
				drawPos = drawPos.RotatedBy(-player.fullRotation);
				drawPos += player.MountedCenter;
				drawPos += Vector2.UnitY * player.gfxOffY; 
				float rotation = (player.gravDir > 0 ? 0 : MathHelper.Pi) - player.fullRotation;

				var count = 0;
				for (var i = Imbuable.SlotIndexID.Passive; i < imbue.Skills.Length; i++)
				{ 
					if (imbue.Skills[i] is not null || !imbue.cachedSpells[i].IsNullOrWhiteSpace())
					{
						count++;
					}
				}

				var secondaryItemPos = drawPos - new Vector2(dimensions.Width * (count - 1f) / 2f, 0).RotatedBy(-player.fullRotation);

				if (imbue.Passive is not null)
				{
					var texture = backgroundSprites.Item1.Value;
						
					var colour = Color.White;
					if (!imbue.PassiveActive)
					{
						colour *= .75f;
						texture = backgroundSprites.Item2.Value;
					}
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex;
					if (imbue.Passive.Scroll != 0)
						tex = TextureAssets.Item[imbue.Passive.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
				}
				else if (!imbue.cachedSpells[Imbuable.SlotIndexID.Passive].IsNullOrWhiteSpace())
				{
					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedItem>()];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
				}

				if (imbue.Mobility is not null)
				{
					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex;
					if (imbue.Mobility.Scroll != 0)
						tex = TextureAssets.Item[imbue.Mobility.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
				}
				else if (!imbue.cachedSpells[Imbuable.SlotIndexID.Mobility].IsNullOrWhiteSpace())
				{
					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedItem>()];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
				}

				if (imbue.Dash is not null)
				{
					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex;
					if (imbue.Dash.Scroll != 0)
						tex = TextureAssets.Item[imbue.Dash.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
				}
				else if (!imbue.cachedSpells[Imbuable.SlotIndexID.Dash].IsNullOrWhiteSpace())
				{
					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedItem>()];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
				}

				if (count > 0)
					drawPos.Y -= dimensions.Height;

				for (int i = 0; i < 3; i++)
				{
					var pos = drawPos;
					switch (i)
					{
						case 0:
							pos -= new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
							break;
						case 2:
							pos += new Vector2(dimensions.Width, 0).RotatedBy(-player.fullRotation);
							break;
					}
					var spell = imbue.Skills[i];
					if (spell is not null)
					{
						var colour = Color.White;

						Texture2D texture = backgroundSprites.Item1.Value;

						if (imbue.selectedIndex != i)
						{
							colour *= .75f;
							texture = backgroundSprites.Item2.Value;
						}

						DrawData a = new(texture, pos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex;
						if (spell.Scroll != 0)
							tex = TextureAssets.Item[spell.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, (MathHelper.Min(dimensions.Height, dimensions.Width) - 4f) / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawDatas.AddRange(a, d);
					}
					else if (!imbue.cachedSpells[i].IsNullOrWhiteSpace())
					{
						var colour = Color.White;

						Texture2D texture = backgroundSprites.Item1.Value;

						if (imbue.selectedIndex != i)
						{
							colour *= .75f;
							texture = backgroundSprites.Item2.Value;
						}

						DrawData a = new(texture, pos - Main.screenPosition, texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedItem>()];

						DrawData d = new(tex.Value, pos - Main.screenPosition, tex.Frame(), colour, rotation, tex.Size() / 2f, (MathHelper.Min(dimensions.Height, dimensions.Width) - 4f) / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawDatas.AddRange(a, d);
					}
				}
					
				drawInfo.DrawDataCache.AddRange(drawDatas);
			}
		}
	}
}

