using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Items.Weapons.Scrolls;
using ArcaneOdyssey.Content.NPCS;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey
{
	public class ArcaneOdysseyMod : Mod
	{
		/// <summary>
		/// disable all cooldowns and stuff lmao
		/// </summary>
		public static bool devMode => DevMode.devMode;
		public const string InternalName = "ArcaneOdyssey";

		public static ArcaneOdysseyMod Instance => ModContent.GetInstance<ArcaneOdysseyMod>();

		internal static Dictionary<string, LocalizedText> staticLocalizer = [];
		internal static Dictionary<int, bool?> coldItems = [];
		internal static List<int> excludedItems = [];
		internal static List<int> excludedProjectiles = [];

		public override object Call(params object[] args)
		{
			switch (args[0])
			{
				case "BlacklistProjectile":
				case "ExcludeProjectile":
					excludedProjectiles.Add((int)args[1]);
					break;
				case "BlacklistItem":
				case "ExcludeItem":
					excludedItems.Add((int)args[1]);
					break;
				case "GetPlayerImbue":
					AOPlayer player = Main.player[(int)args[1]].ArcaneOdyssey();
					return player.Imbue.Type;
					break;
				case "GetItemImbue":
					Imbuable imbue = new Item((int)args[1]).ArcaneOdyssey().Imbue;
					return imbue.Type;
					break;
				case "RegisterItemTemperature":
				case "AddItemTemperature":
					coldItems.Add((int)args[1], (bool?)args[2]);
					break;
				case "GetItemTemperature":
					var item1 = args[1] as Item;
					return item1.ArcaneOdyssey().Cold;
					break;
			}
			return null;
		}
	}

	public class WorldGenStuff : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			// Tucker died lmao
			int Stalac = tasks.FindIndex(genpass => genpass.Name == "Stalac");
			if (ArcaneOdysseyClientConfig.Instance.GenerateTucker && Stalac != -1)
			{
				tasks.Insert(Stalac + 1, new PassLegacy("Tucker Grave", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Tucker").Value;
					KillTucker(Main.spawnTileX - 2, Main.spawnTileY - 2, Main.spawnTileX + 2, Main.spawnTileY + 2, TileID.Tombstones);
				}));
			}

			int guide = tasks.FindIndex(genpass => genpass.Name == "Guide");
			if (ArcaneOdysseyConfig.Instance.EnableMorden && guide != -1)
			{
				tasks.Insert(Stalac + 1, new PassLegacy("Morden", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Morden").Value;
					SpawnMorden();
				}));
			}
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
						WorldGen.PlaceObject(x, y, tile, false, 2, 0, -1, Utils.NextBool(WorldGen.genRand, 2) ? 1 : -1);
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
					if (Main.rand.NextBool(6000))
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

					int[] oldItems = [ModContent.ItemType<OldRapier>(), ModContent.ItemType<OldSword>(), ModContent.ItemType<OldGreataxe>(), ModContent.ItemType<OldGreatsword>(), ModContent.ItemType<WoodenStaff>(),];
					if (chest.y > Main.rockLayer && chest.y < Main.UnderworldLayer && !chest.IsLocked()) // cavern chests probably
					{
						if (Main.rand.Next(Enumerable.Range(0, oldItems.Length).ToArray()) != 0)
						{
							for (int i = 0; i < Chest.maxItems; i++)
							{
								if (chest.item[i] != null && chest.item[i].IsAir)
								{
									chest.item[i].SetDefaults(Main.rand.Next(oldItems));
									break;
								}
							}
						}

						if (Main.rand.NextBool(10))
						{
							for (int i = 0; i < Chest.maxItems; i++)
							{
								if (chest.item[i] != null && chest.item[i].IsAir)
								{
									chest.item[i].SetDefaults(ModContent.ItemType<CannonScroll>());
									break;
								}
							}
						}
					}

					if (chest.y > Main.UnderworldLayer && chest.IsLocked()) // shadow chests
					{

						if (Main.rand.NextBool(5))
						{
							for (int i = 0; i < Chest.maxItems; i++)
							{
								if (chest.item[i] != null && chest.item[i].IsAir)
								{
									chest.item[i].SetDefaults(ModContent.ItemType<PulsarScroll>());
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	public class DevMode : ModSystem { public static bool devMode = false; }

	public class AODebuffManager : GlobalBuff
	{
		public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
		{
			buffName = buffName.Replace("Imbue", "Gel");
		}
	}

	public class DownedBosses : ModSystem
	{
		public static bool downedEvander;
		public static bool downedEnragedEmpress;
		public static bool downedWorldEater;
		public static bool downedBrain;

		public static void ResetDefaults()
		{
			downedEvander = false;
			ExternalModSupport.hasYapped = false;
			downedEnragedEmpress = false;
		}

		public override void OnWorldLoad() => ResetDefaults();

		public override void OnWorldUnload() => ResetDefaults();

		public override void SaveWorldData(TagCompound tag)
		{
			List<string> downed = [];
			if (downedEvander)
				downed.Add("Evander");
			if (downedEnragedEmpress)
				downed.Add("EnragedEoL");

			tag["downed"] = downed;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedEvander = downed.Contains("Evander");
			downedEnragedEmpress = downed.Contains("EnragedEoL");
		}
	}

	public class DownedNPCTracker : GlobalNPC
	{
		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.HallowBoss)
			{
				//if (npc.AI_120_HallowBoss_IsGenuinelyEnraged())
				//{
				//    DownedBosses.downedEnragedEmpress = true;
				//    if (Main.dedServ)
				//    {
				//        NetMessage.SendData(MessageID.WorldData);
				//    }
				//}
			}

			if (npc.type == NPCID.EaterofWorldsHead)
			{
				DownedBosses.downedWorldEater = true;
				if (Main.dedServ)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}

			if (npc.type == NPCID.BrainofCthulhu)
			{
				DownedBosses.downedBrain = true;
				if (Main.dedServ)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}
		}
	}
}
