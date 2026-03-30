using ArcaneOdyssey.Items.Armour.Vanity.Masks;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Items.Weapons.Sunken;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.NPCs.Minibosses;
using ArcaneOdyssey.NPCs.Town;
using ArcaneOdysseyMusic.MusicBoxes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class ExternalModSupport : ModSystem
	{
		public override void PostSetupContent()
		{
			AddFargosStuff();
			AddShieldSlots();
			MiscCalamitysStuff();
			AddBossChecklist();
		}

		public static void RegisterDebuff(ModBuff buff)
		{
			if (HasCalamity)
			{
				var call = (NPC e) => e.HasBuff(buff.Type);
				Calamity.Call("RegisterDebuff", buff.Texture, call);
			}
		}

		public void MiscCalamitysStuff()
		{
			if (!HasCalamity)
				return;

			string[] descs = [Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description1").Value, Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description2").Value, Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description3").Value, Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description4").Value];
			string[] descs2 = [Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description1").Value, Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description2").Value, Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description3").Value, Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description4").Value];
			Calamity.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Name").Value,
				string.Join(' ', descs),
				() => true);
			Calamity.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Name").Value,
				string.Join(' ', descs2),
				() => true);
		}

		public static void DeclareMiniboss(int type)
		{
			if (HasCalamity)
				Calamity.Call("DeclareMiniboss", type);
		}

		public static void AddShieldSlots()
		{
			if (ModLoader.TryGetMod("ShieldSlot", out Mod shieldSlot))
			{
				shieldSlot.Call(ModContent.ItemType<ReflexScroll>());
			}
		}

		public static bool CanDoubleTapDash()
		{
			if (HasCalamity)
			{
				return DashBind().GetAssignedKeys().Count == 0;
			}
			if (HasFargos)
			{
				return !(bool)Fargos.Call("DoubleTapDashDisabled");
			}
			return true;
		}

		public static ModKeybind DashBind()
		{
			if (HasCalamity)
			{
				var a = Calamity.Code.GetType("CalamityMod.CalamityKeybinds");
				if (a is not null)
				{
					return (ModKeybind)a.GetProperty("DashHotkey").GetValue(null);
				}
			}
			else if (HasFargos)
			{
				var e = Fargos.GetType().
					GetField("DashKey").
					GetValue(null);
				return (ModKeybind)e;
			}
			return null;
		}

		private void AddFargosStuff()
		{
			if (HasFargos)
			{
				// stat sheet
				Func<string> SizeText = () => Mod.CustomLocalization("FargosSheet.SizeMulti", Math.Round(100f * Main.LocalPlayer.ArcaneOdyssey().SizeMulti - 100f, 1)).Value;
				Fargos.Call("AddStat", ModContent.ItemType<ColossalGreatsword>(), SizeText);
				Func<string> HasteStat = () => Mod.CustomLocalization("FargosSheet.CooldownMulti", Math.Abs(Math.Round(100f * Main.LocalPlayer.ArcaneOdyssey().CooldownDurationMulti - 100f, 1))).Value;
				Fargos.Call("AddStat", ModContent.ItemType<SunkenSword>(), HasteStat);

				//Func<string> blood = () => Mod.CustomLocalization("FargosSheet.BloodDisease", Main.LocalPlayer.ArcaneOdyssey().BloodDiseaseName).Value;
				//Fargos.Call("AddStat", ItemID.PsychoKnife, blood);

				Fargos.Call("AddDevianttHelpDialogue", "Deviantt", (byte)2, (string _) => "No Conditions", $"{Mod.Name}.NPCs.Town.{nameof(Edgelord)}");
			}
		}

		public static bool HasCalamity => ModLoader.HasMod("CalamityMod");
		public static Mod Calamity => ModLoader.GetMod("CalamityMod");
		public static bool HasFargos => ModLoader.HasMod("Fargowiltas");
		public static Mod Fargos => ModLoader.GetMod("Fargowiltas");
		public static bool HasThorium => ModLoader.HasMod("ThoriumMod");
		public static Mod Thorium => ModLoader.GetMod("ThoriumMod");

		private void AddBossChecklist()
		{
			if (!ModLoader.TryGetMod("BossChecklist", out var bossChecklist) || bossChecklist.Version < new Version(1, 6))
			{
				return;
			}

			void EvanderStuff()
			{
				string internalName = nameof(Evander);
				float weight = 7.5f; // right after wof
				Func<bool> downed = () => DownedBosses.downedEvander;
				int bossType = ModContent.NPCType<Evander>();
				int trophy = ModContent.ItemType<EvanderTrophy>();
				LocalizedText spawnInfo = Mod.CoolCustomLocalization($"NPCs.Minibosses.{internalName}.SpawnInfo");

				bossChecklist.Call(
				"LogMiniBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["collectibles"] = new List<int> { trophy },
					["spawnInfo"] = spawnInfo
				});
			}

			void DuskStuff()
			{
				string internalName = nameof(Dusk);
				float weight = 3.5f; // right after eow
				Func<bool> downed = () => DownedBosses.downedDusk;
				int bossType = ModContent.NPCType<Dusk>();
				int trophy = ModContent.ItemType<DuskTrophy>();
				int mask = ModContent.ItemType<DuskMask>();
				LocalizedText spawnInfo = Mod.CoolCustomLocalization($"NPCs.Minibosses.{internalName}.SpawnInfo");

				bossChecklist.Call(
				"LogMiniBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["collectibles"] = new List<int> { mask, trophy },
					["spawnInfo"] = spawnInfo
				});
			}

			void LaelusStuff()
			{
				string internalName = nameof(Laelus);
				float weight = .5f; // right away!
				Func<bool> downed = () => DownedBosses.downedLaelus;
				int bossType = ModContent.NPCType<Laelus>();
				int trophy = ModContent.ItemType<LaelusTrophy>();
				LocalizedText spawnInfo = Mod.CoolCustomLocalization($"NPCs.Minibosses.{internalName}.SpawnInfo");

				bossChecklist.Call(
				"LogMiniBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["collectibles"] = new List<int> { trophy },
					["spawnInfo"] = spawnInfo
				});
			}

			void EliusStuff()
			{
				string internalName = nameof(LordElius);
				float weight = 2.6f; // after blood moon
				Func<bool> downed = () => DownedBosses.downedElius;
				int bossType = ModContent.NPCType<LordElius>();
				int trophy = ModContent.ItemType<EliusTrophy>();
				int relic = ModContent.ItemType<EliusBossRelic>();
				int pet = ModContent.ItemType<VermillionBracelet>();
				int musicbox = ModContent.ItemType<EliusMusicBox>();
				LocalizedText spawnInfo = Mod.CoolCustomLocalization($"NPCs.Bosses.{internalName}.SpawnInfo");

				bossChecklist.Call(
				"LogBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["collectibles"] = new List<int> { pet, musicbox, relic, trophy },
					["spawnInfo"] = spawnInfo
				});
			}

			EvanderStuff();
			DuskStuff();
			LaelusStuff();
			EliusStuff();

			bossChecklist.Call("SubmitEntryCollectibles", Mod, new Dictionary<string, object>()
			{
				{ "Terraria HallowBoss", new List<int>() { ModContent.ItemType<HecateShard>() } },
			});
		}

		public static bool? CheckItemTemperature(ModItem item)
		{
			if (item.Mod.Name == "CalamityMod") // would do more mods but calamity is just easy since i have the source code
			{
				switch (item.Name)
				{
					case "AbsoluteZero":
					case "AbyssBlade":
					case "AmidiasTrident":
					case "Avalanche":
					case "BrinyBaron":
					case "DepthCrusher":
					case "Floodtide":
					case "NeptunesBounty":
					case "Riptide":
					case "SeashineSword":
					case "Shimmerspark":
					case "StarnightLance":
					case "TenebreusTides":
					case "TyphonsGreed":
					case "UrchinMace":
					case "Alluvion":
					case "AquashardShotgun":
					case "Archerfish":
					case "DarkechoGreatbow":
					case "EternalBlizzard":
					case "FlakKraken":
					case "FlurrystormCannon":
					case "FrostbiteBlaster":
					case "HoarfrostBow":
					case "Leviatitan":
					case "Megalodon":
					case "Monsoon":
					case "SDFMG":
					case "Seadragon":
					case "SeasSearing":
					case "TheMaelstrom":
					case "ShardlightPickaxe":
					case "AbyssalWarhammer":
						return true;
					case "AegisBlade":
					case "AnarchyBlade":
					case "BalefulHarvester":
					case "Brimlance":
					case "Brimlash":
					case "BrimstoneSword":
					case "BurningRevelation":
					case "DevilsSunrise":
					case "DraconicDestruction":
					case "DragonPow":
					case "DragonRage":
					case "EssenceFlayer":
					case "FaultLine":
					case "HellfireFlamberge":
					case "HolyCollider":
					case "MawOfInfinity":
					case "Mourningstar":
					case "OldLordClaymore":
					case "SeekingScorcher":
					case "StreamGouge":
					case "TheBurningSky":
					case "UltimusCleaver":
					case "VulcaniteLance":
					case "AuroraBlazer":
					case "BlissfulBombardier":
					case "BloodBoiler":
					case "BrimstoneFury":
					case "ChickenCannon":
					case "ChromaticEruption":
					case "ContinentalGreatbow":
					case "DaemonsFlame":
					case "DeadSunsWind":
					case "DragonsBreath":
					case "Drataliornus":
					case "FirestormCannon":
					case "FlarewingBow":
					case "HalleysInferno":
					case "HavocsBreath":
					case "Hellborn":
					case "Helstorm":
					case "MagnomalyCannon":
					case "Meowthrower":
					case "PristineFury":
					case "TelluricGlare":
					case "DragoonDrizzlefish":
					case "WildfireBloom":
					case "InfernaCutter":
					case "SeismicHampick":
					case "TectonicTruncator":
						return false;
				}
			}
			return null;
		}

		public static WeaponType CheckWeaponsType(ModItem item)
		{
			if (item.Mod.Name == "CalamityMod") // would do more mods but calamity is just easy since i have the source code
			{
				switch (item.Name)
				{
					case "ClockworkBow":
					case "FlakKraken":
					case "HandheldTank":
					case "MarksmanBow":
					case "Roxcalibur":
					case "DeepcoreGK2":
					case "AnarchyBlade":
					case "GrandGuardian":
					case "HolyCollider":
					case "MajesticGuard":
						return WeaponType.Strength;
					case "Karasawa":
					case "PrismaticBreaker":
					case "TheBurningSky":
						return WeaponType.Arcanium;
					case "TrueBiomeBlade":
					case "BrokenBiomeBlade":
					case "OmegaBiomeBlade":
					case "Galaxia":
					case "FourSeasonsGalaxia":
					case "ArkoftheCosmos":
					case "ArkoftheElements":
					case "FracturedArk":
					case "SkytideDragoon":
					case "Earth":
					case "TrueArkoftheAncients":
					case "Orderbringer":
					case "GreatswordofJudgement":
						return WeaponType.Artisinal;
				}
			}
			return WeaponType.Normal;
		}
	}
}
