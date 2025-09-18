using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.NPCS;
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
using Terraria.WorldBuilding;

namespace ArcaneOdyssey
{
	public class ArcaneOdyssey : Mod // what does bro even do lmao
	{
		public static Dictionary<string, LocalizedText> staticLocalizer = [];
	}

	public class FirstCultistKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedAncientCultist;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{nameof(ArcaneOdyssey)}.FirstCultistKillDescription", () => "First Lunatic Cultist Defeated").Value;
	}


	public class AOPlayer : ModPlayer
	{
		public AOMagic imbue = null;

		/// <summary>
		/// Whether the user has a set of sunken armour equipped
		/// </summary>
		public bool sunkenArmour = false;

		public int AOSizeStat = 0;

		public Projectile myCircle = null;
		public float StunCD = 0;
		public bool RightClicking => Player.altFunctionUse == 2;

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				return [new Item(ModContent.ItemType<PoseidonChoice>()), new Item(ModContent.ItemType<TitleMusicBox>())];
			}
			else return [];
		}

		public override void ResetEffects()
		{
			sunkenArmour = false;
			AOSizeStat = 0;
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (sunkenArmour)
			{
				npc.AddBuff(BuffID.Wet, 60 * 10);
			}
		}

		public float GetSizeMulti(Item item)
		{
			float stat = AOSizeStat / 300f;
			if (Player.meleeScaleGlove && item.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			return stat+1;
		}

		public float GetSizeMulti(Projectile projectile)
		{
			float stat = AOSizeStat / 300f;
			if (Player.meleeScaleGlove && projectile.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			return stat + 1f;
		}

        public override void PreUpdate()
        {
			if (Main.LocalPlayer == Player)
				StunCD -= 1 / 60;
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
				tasks.Insert(Stalac + 1, new PassLegacy("Tucker Grave", (progress, config) =>
				{
					progress.Message = Mod.CustomLocalization("WorldGen.Tucker").Value;
					WorldGenTasks.KillTucker(Main.spawnTileX - 2, Main.spawnTileY - 2, Main.spawnTileX + 2, Main.spawnTileY + 2, TileID.Tombstones);
				}));

            int guide = tasks.FindIndex(genpass => genpass.Name == "Guide");
			if (guide != -1)
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
}
