using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Armour.Vanity.Masks;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Items.Scrolls.Usable.Rare;
using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Items.Weapons.Sunken;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.NPCs.Minibosses;
using ArcaneOdyssey.NPCs.Town;
using ArcaneOdysseyMusic.MusicBoxes;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
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
			MagicStorageSupport();
		}

		public static void MagicStorageSupport()
		{
			if (!HasMS)
				return;

			RegisterShadowDiamondDrop(ModContent.NPCType<LordElius>());
		}

		private static IItemDropRule GetShadowDiamondDropRule(int normal = 1, int expert = -1)
		{
			return (IItemDropRule)MS.Call(
			
				"Get Shadow Diamond Drop Rule",
				normal,
				expert
			);
		}

		private static void SetShadowDiamondDropRule(int npcType, IItemDropRule rule)
		{
			MS.Call(
				"Set Shadow Diamond Drop Rule",
				npcType,
				rule
			);
		}

		private static void RegisterShadowDiamondDrop(int npcType, int normal = 1, int expert = -1)
		{
			SetShadowDiamondDropRule(npcType, GetShadowDiamondDropRule(normal, expert));
		}

		private static void RegisterShadowDiamondDropNormal(int npcType, int amount = 1)
		{
			IItemDropRule diamondDropRule = GetShadowDiamondDropRule(amount, -1);
			IItemDropRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			Chains.OnSuccess(notExpertRule, diamondDropRule, false);
			SetShadowDiamondDropRule(npcType, notExpertRule);
		}

		private static void RegisterShadowDiamondDropDummy(int npcType, int amount = 1)
		{
			IItemDropRule diamondDropRule = GetShadowDiamondDropRule(amount, -1);
			IItemDropRule dummyRule = new LeadingConditionRule(new Conditions.NeverTrue());
			dummyRule.OnSuccess(diamondDropRule, false);
			SetShadowDiamondDropRule(npcType, dummyRule);
		}

		public static bool Mastvengence
		{
			get
			{
				if (HasCalamity)
				{
					if ((bool)Calamity.Call("DifficultyActive", "revengeance"))
					{
						return true;
					}
				}
				return Main.masterMode;
			}
		}

		public static void RegisterDebuff(ModBuff buff)
		{
			var call = (NPC e) => e.HasBuff(buff.Type);
			Calamity?.Call("RegisterDebuff", buff.Texture, call);
		}

		public static void RegisterDoT(int type)
		{
			Thorium?.Call("AddPlayerDoTBuffID", type);
		}

		public static void RegisterStatusBuff(int type)
		{
			Thorium?.Call("AddPlayerStatusBuffID", type);
		}

		public void MiscCalamitysStuff()
		{
			string[] descs = [Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description1").Value, Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description2").Value, Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description3").Value, Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Description4").Value];
			string[] descs2 = [Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description1").Value, Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description2").Value, Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description3").Value, Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Description4").Value];
			Calamity?.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.DarkSea.Name").Value,
				string.Join(' ', descs),
				() => true);
			Calamity?.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.Epicentre.Name").Value,
				string.Join(' ', descs2),
				() => true);
		}

		public static void DeclareMiniboss(int type)
		{
			Calamity?.Call("DeclareMiniboss", type);
		}

		public static void ThoriumStuff()
		{
			if (!HasThorium)
				return;
			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				var item = new Item(i);
				if ((bool)Thorium.Call("IsFlailProjectileID", item.shoot))
				{
					ArcaneOdysseyMod.Sets.flail[i] = true;
				}
			}

			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				if (ArcaneOdysseyMod.Sets.flail[i])
				{
					var item = ModContent.GetModItem(i);
					if (item?.Mod is ArcaneOdysseyMod)
					{
						Thorium.Call("AddFlailProjectileID", item.Item.shoot);
					}
				}
			}
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
			// stat sheet
			Func<string> SizeText = () => Mod.CustomLocalization("FargosSheet.SizeMulti", Math.Round(100f * Main.LocalPlayer.ArcaneOdyssey().SizeMulti - 100f, 1)).Value;
			Fargos?.Call("AddStat", ModContent.ItemType<ColossalGreatsword>(), SizeText);
			Func<string> HasteStat = () => Mod.CustomLocalization("FargosSheet.CooldownMulti", Math.Abs(Math.Round(100f * Main.LocalPlayer.ArcaneOdyssey().CooldownDurationMulti - 100f, 1))).Value;
			Fargos?.Call("AddStat", ModContent.ItemType<SunkenSword>(), HasteStat);

			//Func<string> blood = () => Mod.CustomLocalization("FargosSheet.BloodDisease", Main.LocalPlayer.ArcaneOdyssey().BloodDiseaseName).Value;
			//Fargos.Call("AddStat", ItemID.PsychoKnife, blood);

			Fargos?.Call("AddDevianttHelpDialogue", "Deviantt", (byte)2, (string _) => "No Conditions", $"{Mod.Name}.NPCs.Town.{nameof(Edgelord)}");
			Fargos?.Call("AddIndestructibleRectangle", EliusArenaLoader.eliusArena.ToWorldRect());
			Fargos?.Call("AddPermaUpgrade", new Item(ModContent.ItemType<AcumenTechnique>()), () => Main.LocalPlayer.ArcaneOdyssey().acumen);
		}

		public static bool HasCalamity => ModLoader.HasMod("CalamityMod");
		public static Mod Calamity => HasCalamity ? ModLoader.GetMod("CalamityMod") : null;
		public static bool HasFargos => ModLoader.HasMod("Fargowiltas");
		public static Mod Fargos => HasFargos ? ModLoader.GetMod("Fargowiltas") : null;
		public static bool HasThorium => ModLoader.HasMod("ThoriumMod");
		public static Mod Thorium => HasThorium ? ModLoader.GetMod("ThoriumMod") : null;
		public static bool HasMS => ModLoader.HasMod("MagicStorage");
		public static Mod MS => HasMS ? ModLoader.GetMod("MagicStorage") : null;

		public static bool NotInSubworld
		{
			get
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
				return false;
			}
		}

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
				Func<bool> downed = () => DownedBosses.DownedEvander;
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
				Func<bool> downed = () => DownedBosses.DownedDusk;
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
				Func<bool> downed = () => DownedBosses.DownedLaelus;
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
				Func<bool> downed = () => DownedBosses.DownedElius;
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
					case "HalleysInferno":
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
			if (item.Mod.Name == "ThoriumMod")
			{
				if (item.Name == "StellarSystem")
				{
					//save progress
				}

				switch (item.Name)
				{
					case "HydroPump":
					case "TheWhirlpool":
					case "Chum":
					case "WhirlpoolSaber":
					case "IcyGaze":
					case "DeitysTrefork":
					case "OceansJudgement":
					case "SevenSeasDevastator":
					case "TidalWave":
					case "SeahorseWand":
					case "BlobhornCoralStaff":
					case "GeyserStaff":
					case "SeaFoamScepter":
					case "ClimbersIceAxe":
					case "SpiritBreaker":
					case "Freeze":
					case "NitrogenVial":
						return true;
					case "TheSeaMine":
					case "GodKiller":
					case "AlmanacofAgony":
					case "DevilsClaw":
					case "EmberStaff":
					case "PrometheanStaff":
					case "DraconicMagmaStaff":
					case "EruptingFlare":
					case "EssenceofFlame":
					case "GoldenLocks":
					case "GolemsGaze":
					case "HellishHalberd":
					case "HellfireMinigun":
					case "ObsidianStaff":
					case "InfernalAnimator":
					case "TheMassacre":
					case "Ignite":
					case "InfernoStaff":
					case "DoomFireAxe":
					case "SolScorchedSlab":
					case "CinderString":
					case "CometCrossfire":
					case "MeteorHeadStaff":
					case "CombustionFlask":
					case "MoltenKnife":
					case "MeteoriteClusterBomb":
					case "PlasmaVial":
						return false;
				}

				switch (item.GetType().Namespace.Split('.')[^1])
				{
					case "Icy":
						return true;
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
					case "Karasawa":
					case "GrandDad":
						return WeaponType.Strength;
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
			if (item.Mod.Name == "ThoriumMod")
			{
				switch (item.Name)
				{
					case "MastersLibram":
					case "QuasarsFlare":
					case "SnowWhite":
					case "StellarSystem":
					case "UselessStaff":
					case "WondrousWand":
					case "EclipseFang":
						return WeaponType.Artisinal;
					case "TerrariansLastKnife":
					case "WyvernSlayer":
					case "QuakeGauntlet":
						return WeaponType.Strength;
				}
			}
			return WeaponType.Normal;
		}

		public static void CheckWeapon(ModItem item)
		{
			if (item.Mod.Name == "CalamityMod") // would do more mods but calamity is just easy since i have the source code
			{
				if (item.Name.Contains("greatsword", StringComparison.CurrentCultureIgnoreCase) || item.Name.Contains("claymore", StringComparison.CurrentCultureIgnoreCase))
				{
					ArcaneOdysseyMod.Sets.greatsword[item.Type] = true;
				}
				if (item.Name.Contains("greataxe", StringComparison.CurrentCultureIgnoreCase))
				{
					ArcaneOdysseyMod.Sets.greataxe[item.Type] = true;
				}
				if (item.Name.Contains("knives", StringComparison.CurrentCultureIgnoreCase))
				{
					ArcaneOdysseyMod.Sets.dagger[item.Type] = true;
				}
				if (item.Name.Contains("claws", StringComparison.CurrentCultureIgnoreCase))
				{
					ArcaneOdysseyMod.Sets.claw[item.Type] = true;
				}
				switch (item.Name)
				{
					case "AegisBlade":
					case "AnarchyBlade":
					case "Ataraxia":
					case "BlightedCleaver":
					case "CelestialClaymore":
					case "CometQuasher":
					case "DevilsDevastation":
					case "DraconicDestruction":
					case "Earth":
					case "GalactusBlade":
					case "GrandDad":
					case "GrandGuardian":
					case "Hellkite":
					case "HolyCollider":
					case "MajesticGuard":
					case "Roxcalibur":
					case "StellarStriker":
					case "StormRuler":
					case "TheMutilator":
					case "VoidEdge":
						ArcaneOdysseyMod.Sets.greatsword[item.Type] = true;
						break;
					case "Avalanche":
					case "SeekingScorcher":
						ArcaneOdysseyMod.Sets.greataxe[item.Type] = true;
						break;
					case "EmpyreanKnives":
					case "IllustriousKnives":
					case "TheDarkMaster":
						ArcaneOdysseyMod.Sets.dagger[item.Type] = true;
						break;
					case "FallenPaladinsHammer":
					case "GalaxySmasher":
					case "Pwnagehammer":
					case "StellarContempt":
					case "TriactisTruePaladinianMageHammerofMight":
						ArcaneOdysseyMod.Sets.greathammer[item.Type] = true;
						break;
					case "GildedProboscis":
					case "SkytideDragoon":
					case "StreamGouge":
					case "TheBurningSky":
					case "Violence":
						ArcaneOdysseyMod.Sets.spear[item.Type] = true;
						break;
					case "SaharaSlicers":
						ArcaneOdysseyMod.Sets.dualbladed[item.Type] = true;
						break;
					case "TyphonsGreed":
						ArcaneOdysseyMod.Sets.staff[item.Type] = true;
						break;
				}
			}
			if (item.Mod.Name == "ThoriumMod")
			{
				switch (item.Name)
				{
					case "Rapier":
						ArcaneOdysseyMod.Sets.rapier[item.Type] = true;
						break;
					case "BloodyHighClaws":
						ArcaneOdysseyMod.Sets.claw[item.Type] = true;
						break;
					case "Spearmint":
						ArcaneOdysseyMod.Sets.spear[item.Type] = true;
						break;
					case "WyvernSlayer":
						ArcaneOdysseyMod.Sets.greatsword[item.Type] = true;
						break;
					case "MagicThorHammer":
					case "RangedThorHammer":
					case "MeleeThorHammer":
						ArcaneOdysseyMod.Sets.greathammer[item.Type] = true;
						break;
					case "LodeStoneGreatAxe":
						ArcaneOdysseyMod.Sets.greataxe[item.Type] = true;
						break;
				}
			}
		}
	}

	public class TooltipTweaks : GlobalItem
	{
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (ExternalModSupport.HasCalamity) 
			{
				if (item.ModItem?.Mod == Mod)
				{
					var master = tooltips.Find(e => e.Mod == "Terraria" && e.Name == "Master");
					if (master is not null)
					{
						master.Text = Language.GetTextValue("Mods.CalamityMod.Vanilla.MasterExclusive");
					}
				} 
			}
		}
	}
}
