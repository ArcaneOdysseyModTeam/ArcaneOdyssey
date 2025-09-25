using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.NPCS;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey
{
	public class ArcaneOdyssey : Mod // what does bro even do lmao
	{
		public static Dictionary<string, LocalizedText> staticLocalizer = [];

		public static List<int> ExcludedItems = [];
		public static List<int> ExcludedProjectiles = [];

		public override object Call(params object[] args)
		{
			switch (args[0])
			{
				case "ExcludeProjectile":
					ExcludedProjectiles.Add((int)args[1]);
					return null;
					break;
				case "ExcludeItem":
					ExcludedItems.Add((int)args[1]);
					return null;
					break;
				case "GetImbue":
					AOPlayer player = Main.player[(int)args[1]].ArcaneOdyssey();
					return player.imbue.Type;
					break;
				default:
					return null;
			}
		}
	}

	public class FirstCultistKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedAncientCultist;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{nameof(ArcaneOdyssey)}.FirstCultistKillDescription", () => "First Lunatic Cultist Defeated").Value;
	}


	public class AOPlayer : ModPlayer
	{
		public Imbuable imbue = null;
		public bool chargingSpell = false;

		public int AOSizeStat = 0;

		public Projectile myCircle = null;
		public bool RightClicking => Player.altFunctionUse == 2;

		public Dictionary<string, int> Cooldowns = [];
		public Dictionary<int, int> BuffCooldowns = [];
		public Dictionary<int, int> ItemCooldowns = [];

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				List<Item> items = [
					new Item(ModContent.ItemType<PoseidonChoice>()), 
					new Item(ModContent.ItemType<TitleMusicBox>()), 
					new Item(ModContent.ItemType<EaglePatrimony>())];
				if (Main.expertMode)
				{
					items.Add(new Item(ModContent.ItemType<Acrimony>()));
				}
				return items;
			}
			else return [];
		}

		public override void PreUpdateMovement()
		{
			if (myCircle is not null && myCircle.ai[1] != 2)
			{
				Player.velocity = Vector2.Zero;
				Player.maxFallSpeed = 0f;
			}
		}

		public override void PostUpdate()
		{
			if (chargingSpell)
				Player.statDefense *= .75f;
		}

		public override void ResetEffects()
		{
			AOSizeStat = 0;
		}

		public float GetSizeMulti(Item item = null)
		{
			float stat = AOSizeStat / 300f;
			if (item is not null && Player.meleeScaleGlove && item.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			stat++;
			return stat;
		}

		public float GetSizeMulti(Projectile projectile)
		{
			float stat = AOSizeStat / 300f;
			if (Player.meleeScaleGlove && projectile.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			stat++;
			return stat;
		}

		public override void PreUpdate()
		{
			foreach (string i in Cooldowns.Keys)
			{
				Cooldowns[i]--;
				if (Cooldowns[i] <= 0)
				{
					Cooldowns.Remove(i);
				}
			}

			foreach (int i in BuffCooldowns.Keys)
			{
				BuffCooldowns[i]--;
				if (BuffCooldowns[i] <= 0)
				{
					BuffCooldowns.Remove(i);
				}
			}

			foreach (int i in ItemCooldowns.Keys)
			{
				ItemCooldowns[i]--;
				if (ItemCooldowns[i] <= 0)
				{
					ItemCooldowns.Remove(i);
				}
			}
		}
	}

	public class WorldGenTasks
	{
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
	}

	public class WorldGenStuff : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
		{
			// Tucker died lmao
			int Stalac = tasks.FindIndex(genpass => genpass.Name == "Stalac");
			if (ArcaneOdysseyConfig.Instance.GenerateTucker && Stalac != -1)
			{
				tasks.Insert(Stalac + 1, new PassLegacy("Tucker Grave", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Tucker").Value;
					WorldGenTasks.KillTucker(Main.spawnTileX - 2, Main.spawnTileY - 2, Main.spawnTileX + 2, Main.spawnTileY + 2, TileID.Tombstones);
				}));
			}

			int guide = tasks.FindIndex(genpass => genpass.Name == "Guide");
			if (ArcaneOdysseyConfig.Instance.EnableMorden && guide != -1)
			{
				tasks.Insert(Stalac + 1, new PassLegacy("Morden", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Morden").Value;
					WorldGenTasks.SpawnMorden();
				}));
			}
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
							if (chest.item[i] != null)
							{
								chest.item[i].SetDefaults(ModContent.ItemType<Acrimony>());
								break;
							}
						}
					}
				}
			}
		}
	}

	public class DownedBosses : ModSystem
	{
		public static bool downedEvander;

		public static void ResetDefaults()
		{
			downedEvander = false;
		}

		public override void OnWorldLoad() => ResetDefaults();

		public override void OnWorldUnload() => ResetDefaults();

		public override void SaveWorldData(TagCompound tag)
		{
			List<string> downed = [];
			if (downedEvander)
			{
				downed.Add("Evander");
			}

			tag["downed"] = downed;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedEvander = downed.Contains("Evander");
		}
	}
}
