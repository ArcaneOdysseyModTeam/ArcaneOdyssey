using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Biomes
{
	/// <summary>
	/// Djin Ruins
	/// </summary>
	public class EliusArena : ModBiome
	{
		public override int BiomeTorchItemType => ItemID.Torch;

		public override int BiomeCampfireItemType => ItemID.Campfire;

		public override bool IsBiomeActive(Player player)
		{
			var playercoords = player.Hitbox.ToTileRect();
			if (EliusArenaLoader.eliusArena.Intersects(playercoords))
			{
				if (!ModLoader.TryGetMod("SubworldLibrary", out Mod subworld))
				{
					return true;
				}
				else
				{
					if ((bool)subworld.Call("AnyActive", null))
					{
						return true;
					}
				}
			}

#if VSDEBUGMODE
			if (ModLoader.TryGetMod("SubworldLibrary", out var lib) && ((string)lib.Call("Current")) == "SubworldGenTest/DjinRuinsSubworld")
			{
				if (player.Bottom.Y == 1936)
				{
					AOUtils.Kill(player);
				}
				return true;
			}
#endif
			return false;
		}

		public override void OnInBiome(Player player)
		{
			player.AddBuff(BuffID.NoBuilding, 2); // entirely visual
			if (NPC.downedBoss1)
			{
				if (!AOUtils.BossAlive)
					player.ArcaneOdyssey().EliusArenaCounter++;
				else
					player.ArcaneOdyssey().EliusArenaCounter = 0;

				if (player.ArcaneOdyssey().EliusArenaCounter >= (DownedBosses.downedElius ? (60 * 60) : (60 * 30))) // 30-60 seconds
				{
					if (!AOUtils.BossAlive)
					{
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.SpawnBoss((EliusArenaLoader.eliusArena.Center.X + 25) * 16, (EliusArenaLoader.eliusArena.Center.Y + 2) * 16, ModContent.NPCType<LordElius>(), player.whoAmI);
						}
					}
				}
			}
		}

		public override void OnLeave(Player player)
		{
			player.ArcaneOdyssey().EliusArenaCounter = 0;
		}

		public override void OnEnter(Player player)
		{
			player.ArcaneOdyssey().EliusArenaCounter = 0;
		}

		public override int Music => AOMusicTrack.TitleTheme2.MusicSlot; // change to ambient theme later

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		public override string MapBackground => BackgroundPath;
	}

	public class EliusArenaLoader : ModSystem
	{
		/// <summary>
		/// Area of the elius arena, in tile coordinates
		/// </summary>
		public static Rectangle eliusArena;

		public override void LoadWorldData(TagCompound tag)
		{
			if (tag.ContainsKey("eliusarena"))
			{
				eliusArena = tag.GetIntArray("eliusarena").FromIntArray();
				if (eliusArena == default)
					ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added. His arena has not generated. You cannot fight him.");
			}
			else
			{
				ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added. His arena has not generated. You cannot fight him.");
				eliusArena = Rectangle.Empty;
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("eliusarena", eliusArena.ToIntArray());
		}

		public override void Load()
		{
			eliusArena = Rectangle.Empty;
		}

		public override void Unload()
		{
			eliusArena = Rectangle.Empty;
		}
	}
}
