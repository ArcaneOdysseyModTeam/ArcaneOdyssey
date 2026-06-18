using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.NPCs.Town;
using ArcaneOdyssey.Tiles;
using Microsoft.Xna.Framework;
using StructureHelper.API;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey
{
	public class WorldGenStuff : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			// Spawn Tucker grave
			int Stalac = tasks.FindIndex(genpass => genpass.Name == "Stalac");
			if (Stalac != -1)
			{
				tasks.Insert(Stalac + 1, new PassLegacy("Tucker Grave", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Tucker").Value;
					KillTucker(Main.spawnTileX - 20, Main.spawnTileY - 5, Main.spawnTileX + 20, Main.spawnTileY + 5, ModContent.TileType<TuckerGrave>());
				}));
			}

			// Spawn Morden
			int guide = tasks.FindIndex(genpass => genpass.Name == "Guide");
			if (guide != -1)
			{
				tasks.Insert(Stalac + 1, new PassLegacy("Morden", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Morden").Value;
					SpawnMorden();
				}));
			}

			// Elius Arena
			int islandIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
			if (islandIndex != -1)
			{
				tasks.Insert(islandIndex + 1, new PassLegacy("Elius Arena", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.EliusArena").Value;
					SpawnEliusArena();
				}));
			}
		}

		public static bool IsValidSkyPlacementArea(Rectangle area)
		{
			if ((area.Top >= 0) && (area.Left >= 0))
			{
				for (int i = area.Left; i < area.Right; i++)
				{
					for (int j = area.Top; j < area.Bottom; j++)
					{
						if (Main.tile[i, j].TileType == TileID.Cloud || Main.tile[i, j].TileType == TileID.RainCloud || Main.tile[i, j].TileType == TileID.Sunplate)
							return false;
					}
				}
				return true;
			}
			return false;
		}

		public static void KillTucker(int left, int top, int right, int bottom, int tile)
		{
			bool success = false;
			while (!success)
			{
				int attempts = 0;
				while (!success && attempts <= 1000)
				{
					attempts++;
					int x = WorldGen.genRand.Next(left, right + 1);
					int y = WorldGen.genRand.Next(top, bottom + 1);
					if (Framing.GetTileSafely(x, y).TileType != tile)
					{
						WorldGen.PlaceObject(x, y, tile);
					}
					Tile tile1 = Framing.GetTileSafely(x, y); // maybe use later for something
					success = tile1.TileType == tile;
				}
				if (attempts > 1000)
				{
					break;
				}
			}
		}

		public static void SpawnMorden()
		{
			NPC edgelord = NPC.NewNPCDirect(new EntitySource_WorldGen(), Main.spawnTileX * 16, Main.spawnTileY * 16, ModContent.NPCType<Edgelord>());
			edgelord.homeTileX = Main.spawnTileX;
			edgelord.homeTileY = Main.spawnTileY;
			edgelord.direction = 1;
			edgelord.homeless = true;
		}

		public override void PostWorldGen()
		{
			for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				if (chest != null)
				{
					if (WorldGen.genRand.NextBool(100))
					{
						for (int i = 0; i < Chest.maxItems; i++)
						{
							if (chest.item[i] != null && chest.item[i].IsAir)
							{
								chest.item[i].SetDefaults(ModContent.ItemType<Acrimony>());
								break;
							}
						}
					}

					if ((chest.y > Main.rockLayer) && (chest.y < Main.UnderworldLayer) && (!chest.IsLocked()) && (Main.tile[chest.x, chest.y].TileType == TileID.Containers)) // cavern chests probably
					{
						if (WorldGen.genRand.NextBool(5))
						{
							for (int i = 0; i < Chest.maxItems; i++)
							{
								if (chest.item[i] != null && chest.item[i].IsAir)
								{
									chest.item[i].SetDefaults(WorldGen.genRand.Next(AOItem.oldWeapons));
									break;
								}
							}
						}
						if (WorldGen.genRand.NextBool(5))
						{
							for (int i = 0; i < Chest.maxItems; i++)
							{
								if (chest.item[i] != null && chest.item[i].IsAir)
								{
									chest.item[i].SetDefaults(WorldGen.genRand.Next(AOTile.GetAllCommonScrollDrops()));
									break;
								}
							}
						}
					}

					if ((chest.y > Main.rockLayer) && (chest.y < Main.UnderworldLayer) && chest.IsLocked() && (Main.tile[chest.x, chest.y].TileType == TileID.Containers)) // dungeon chests
					{

					}

					if ((chest.y > Main.UnderworldLayer) && chest.IsLocked() && (Main.tile[chest.x, chest.y].TileType == TileID.Containers)) // shadow chests
					{

					}
				}
			}
		}

		public static void SpawnEliusArena()
		{
			var eliusArenaStruct = Generator.GetStructureData("Structures/EliusArena", ArcaneOdysseyMod.Instance);

			int x = Main.maxTilesX;
			int eliusArenaStructPosX;
			int eliusArenaStructPosY;
			Rectangle arenaBounds;

			do
			{
				eliusArenaStructPosX = WorldGen.genRand.Next((x * 0.6f).Round(), (x * 0.9f).Round());
				eliusArenaStructPosY = WorldGen.genRand.Next((eliusArenaStruct.height / 2) + 25, 250);
				if (GenVars.worldSurfaceLow != 0)
					eliusArenaStructPosY = Math.Min(eliusArenaStructPosY, (int)GenVars.worldSurfaceLow - 50);
				arenaBounds = new(eliusArenaStructPosX, eliusArenaStructPosY, eliusArenaStruct.width, eliusArenaStruct.height);
			}
			while (!IsValidSkyPlacementArea(arenaBounds));

			EliusArenaLoader.eliusArena = arenaBounds.Scaled(1.15f);

			Generator.GenerateFromData(eliusArenaStruct, new(eliusArenaStructPosX, eliusArenaStructPosY));
		}
	}
}
