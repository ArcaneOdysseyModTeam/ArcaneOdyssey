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
			return false;
		}

		public override void OnInBiome(Player player)
		{
			player.AddBuff(BuffID.NoBuilding, 2); // entirely visual
			if (NPC.downedBoss1)
			{
				if (!AOUtils.BossAlive)
					player.ArcaneOdyssey().eliusArenaCounter++;
				else
					player.ArcaneOdyssey().eliusArenaCounter = 0;

				if (player.ArcaneOdyssey().eliusArenaCounter >= (30 * 60)) // 30 seconds
				{
					if (Main.raining || !DownedBosses.downedElius)
					{
						if (AOUtils.ServerOrSingleplayer)
						{
							NPC.SpawnBoss((EliusArenaLoader.eliusArena.Center.X + 25) * 16, (EliusArenaLoader.eliusArena.Center.Y + 1) * 16, ModContent.NPCType<LordElius>(), player.whoAmI);
						}
					}
				}
			}
		}

		public override void OnLeave(Player player)
		{
			player.ArcaneOdyssey().eliusArenaCounter = 0;
		}

		public override void OnEnter(Player player)
		{
			player.ArcaneOdyssey().eliusArenaCounter = 0;
		}

		public override int Music => MusicTrack.Djin.MusicSlot;

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
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("eliusarena", eliusArena.ToIntArray());
		}

		public override void Load()
		{
			eliusArena = default;
		}

		public override void Unload()
		{
			eliusArena = default;
		}

		public override void OnWorldLoad()
		{
			eliusArena = default;
		}

		public override void OnWorldUnload()
		{
			eliusArena = default;
		}
	}
}
