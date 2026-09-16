using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Scrolls;
using Fargowiltas;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent;

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
			List<DrawData> drawDatas = [];
			if (player.PlayerItem().ModItem is ArcaniumWeapon weapon)
			{
				int yOffset = 0;
				if (ExternalModSupport.HasFargos && ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset != default && FargosBuffDisplayActive(drawInfo))
				{
					yOffset += ((player.buffType.Where(d => Main.debuff[d]).Except(FargosIgnoredDebuffs).Count() / 10) + 1) * 32;
				}

				Vector2 drawPos;
				float rotation;
				SpriteEffects effects;
				if (player.gravDir > 0)
				{
					drawPos = player.Top;
					effects = SpriteEffects.None;
					rotation = 0;
				}
				else
				{
					drawPos = player.Bottom;
					effects = SpriteEffects.FlipHorizontally;
					rotation = MathHelper.Pi;
				}
				rotation -= drawInfo.rotation;
				drawPos.Y -= (32f + yOffset) * player.gravDir;

				drawPos -= player.MountedCenter;
				drawPos = drawPos.RotatedBy(-drawInfo.rotation);
				drawPos += player.MountedCenter;
				drawPos += Vector2.UnitY * player.gfxOffY;
				var spell = weapon.Attack;
				if (spell is not null)
				{
					var colour = Color.White;

					Texture2D texture = backgroundSprites.Item1.Value;

					if (weapon.Imbue is null)
					{
						colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
						texture = backgroundSprites.Item2.Value;
					}

					DrawData a = new(texture, drawPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, effects, 0);

					Asset<Texture2D> tex;
					if (spell.Scroll != 0)
						tex = TextureAssets.Item[spell.Scroll];
					else
						tex = TextureAssets.Item[weapon.Type];

					DrawData d = new(tex.Value, drawPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.AddRange(a, d);
				}
				else if (!weapon.cachedSpell.IsNullOrWhiteSpace())
				{
					var colour = Color.White * ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedScroll>()];

					DrawData d = new(tex.Value, drawPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, MathHelper.Min(dimensions.Height, dimensions.Width) / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.Add(d);
				}
				else
				{
					var colour = Color.White;

					Texture2D texture = backgroundSprites.Item1.Value;

					if (weapon.Imbue is null)
					{
						colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
						texture = backgroundSprites.Item2.Value;
					}

					DrawData a = new(texture, drawPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, effects, 0);

					Asset<Texture2D> tex = TextureAssets.Item[weapon.Type];

					colour = colour.MultiplyRGBA(weapon.Colour);

					DrawData d = new(tex.Value, drawPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.AddRange(a, d);
				}
			}
			else if (player.PlayerItem().ModItem is Imbuable imbue)
			{
				int yOffset = 0;
				if (ExternalModSupport.HasFargos && FargosBuffDisplayActive(drawInfo))
				{
					yOffset += ((player.buffType.Where(d => Main.debuff[d]).Except(FargosIgnoredDebuffs).Count() / 10) + 1) * 32;
				}

				Vector2 drawPos;
				float rotation;
				SpriteEffects effects;
				if (player.gravDir > 0)
				{
					drawPos = player.Top;
					effects = SpriteEffects.None;
					rotation = 0;
				}
				else
				{
					drawPos = player.Bottom;
					effects = SpriteEffects.FlipHorizontally;
					rotation = MathHelper.Pi;
				}
				rotation -= drawInfo.rotation;
				drawPos.Y -= (32f + yOffset) * player.gravDir;

				drawPos -= player.MountedCenter;
				drawPos = drawPos.RotatedBy(-drawInfo.rotation);
				drawPos += player.MountedCenter;
				drawPos += Vector2.UnitY * player.gfxOffY;

				var count = 0;
				for (var i = Imbuable.SlotIndexID.Passive; i < imbue.Skills.Length; i++)
				{ 
					if (imbue.Skills[i] is not null || !imbue.cachedSpells[i].IsNullOrWhiteSpace())
					{
						count++;
					}
				}

				var secondaryItemPos = drawPos - new Vector2(dimensions.Width * (count - 1f) / 2f, 0).RotatedBy(-drawInfo.rotation);

				if (imbue.Passive is not null)
				{
					var texture = backgroundSprites.Item1.Value;
						
					var colour = Color.White;
					if (Main.LocalPlayer.ArcaneOdyssey()?.Imbue?.Type != imbue?.Type)
					{
						colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
						texture = backgroundSprites.Item2.Value;
					}
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, effects, 0);

					Asset<Texture2D> tex;
					if (imbue.Passive.Scroll != 0)
						tex = TextureAssets.Item[imbue.Passive.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
				}
				else if (!imbue.cachedSpells[Imbuable.SlotIndexID.Passive].IsNullOrWhiteSpace())
				{
					var colour = Color.White * ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedScroll>()];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, MathHelper.Min(dimensions.Height, dimensions.Width) / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.Add(d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
				}

				if (imbue.Mobility is not null)
				{
					var colour = Color.White;

					Texture2D texture = backgroundSprites.Item1.Value;

					if (Main.LocalPlayer.ArcaneOdyssey()?.Imbue?.Type != imbue?.Type)
					{
						colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
						texture = backgroundSprites.Item2.Value;
					}

					DrawData a = new(texture, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, effects, 0);

					Asset<Texture2D> tex;
					if (imbue.Mobility.Scroll != 0)
						tex = TextureAssets.Item[imbue.Mobility.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
				}
				else if (!imbue.cachedSpells[Imbuable.SlotIndexID.Mobility].IsNullOrWhiteSpace())
				{
					var colour = Color.White * ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedScroll>()];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, MathHelper.Min(dimensions.Height, dimensions.Width) / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.Add(d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
				}

				if (imbue.Dash is not null)
				{
					var colour = Color.White;

					Texture2D texture = backgroundSprites.Item1.Value;

					if (Main.LocalPlayer.ArcaneOdyssey()?.Imbue?.Type != imbue?.Type)
					{
						colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
						texture = backgroundSprites.Item2.Value;
					}
					DrawData a = new(texture, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, effects, 0);

					Asset<Texture2D> tex;
					if (imbue.Dash.Scroll != 0)
						tex = TextureAssets.Item[imbue.Dash.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
				}
				else if (!imbue.cachedSpells[Imbuable.SlotIndexID.Dash].IsNullOrWhiteSpace())
				{
					var colour = Color.White * ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;

					Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedScroll>()];

					DrawData d = new(tex.Value, secondaryItemPos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, MathHelper.Min(dimensions.Height, dimensions.Width) / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
					drawDatas.Add(d);
					secondaryItemPos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
				}

				if (count > 0)
					drawPos -= new Vector2(0, dimensions.Height * player.gravDir).RotatedBy(-drawInfo.rotation);

				for (int i = 0; i < 3; i++)
				{
					var pos = drawPos;
					switch (i)
					{
						case 0:
							pos -= new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
							break;
						case 2:
							pos += new Vector2(dimensions.Width, 0).RotatedBy(-drawInfo.rotation);
							break;
					}
					var spell = imbue.Skills[i];
					if (spell is not null)
					{
						var colour = Color.White;

						Texture2D texture = backgroundSprites.Item1.Value;

						if (imbue.selectedIndex != i)
						{
							colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
							texture = backgroundSprites.Item2.Value;
						}

						DrawData a = new(texture, pos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), texture.Frame(), colour, rotation, texture.Size() / 2f, 1f, effects, 0);

						Asset<Texture2D> tex;
						if (spell.Scroll != 0)
							tex = TextureAssets.Item[spell.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset), tex.Frame(), colour, rotation, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
						drawDatas.AddRange(a, d);
					}
					else if (!imbue.cachedSpells[i].IsNullOrWhiteSpace())
					{
						var colour = Color.White;

						if (imbue.selectedIndex != i)
						{
							colour *= ArcaneOdysseyClientConfig.Instance.UnselectedScrollOpacity;
						}

						Asset<Texture2D> tex = TextureAssets.Item[ModContent.ItemType<UnloadedScroll>()];

						DrawData d = new(tex.Value, pos - Main.screenPosition + (Main.ScreenSize.ToVector2() * ArcaneOdysseyClientConfig.Instance.ImbueSkillsDisplayLocationOffset) , tex.Frame(), colour, rotation, tex.Size() / 2f, MathHelper.Min(dimensions.Height, dimensions.Width) / MathHelper.Max(tex.Width(), tex.Height()), effects, 0);
						drawDatas.Add(d);
					}
				}
			}

			drawInfo.DrawDataCache.AddRange(drawDatas);
		}
	}
}

