using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Armour.Vanity.Masks;
using ArcaneOdyssey.Items.BossRelics;
using ArcaneOdyssey.Items.BossTrophies;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Items.Weapons.Sunken;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.NPCs.Minibosses;
using ArcaneOdyssey.NPCs.Town;
using ArcaneOdysseyMusic.MusicBoxes;
using FargosMod = Fargowiltas.Fargowiltas;
using System;
using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;
using ArcaneOdyssey.Items.Scrolls.Attacks.Rare;

namespace ArcaneOdyssey
{
	public class ExternalModSupport : ModSystem
	{
		public override void PostSetupContent()
		{
			AddFargosStuff();
			AddShieldSlots();
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

		public static void RegisterDebuff(ModBuff buff) { } // unused

		public static void RegisterDoT(int type) => Thorium?.Call("AddPlayerDoTBuffID", type);

		public static void RegisterStatusBuff(int type) => Thorium?.Call("AddPlayerStatusBuffID", type);

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
				//shieldSlot.Call(ModContent.ItemType<ReflexScroll>());
			}
		}

		public static bool CanDoubleTapDash()
		{
			if (HasFargos)
			{
				return !(bool)Fargos.Call("DoubleTapDashDisabled");
			}
			return true;
		}

		public static ModKeybind DashBind
		{
			get
			{
				if (HasFargos)
				{
					return FargosDash;
				}
				return null;
			}
		}

		[JITWhenModsEnabled("Fargowiltas")]
		public static ModKeybind FargosDash => FargosMod.DashKey;

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
			Fargos?.Call("AddPermaUpgrade", new Item(ModContent.ItemType<AcumenTechnique>()), () => Main.LocalPlayer.ArcaneOdyssey().acumen);
		}

		private static bool rectsRegistered = false;

		public override void OnWorldLoad()
		{
			rectsRegistered = false;
		}

		public override void OnWorldUnload()
		{
			rectsRegistered = false;
		}

		public override void PostUpdateEverything()
		{
			if (!rectsRegistered)
			{
				Fargos?.Call("AddIndestructibleRectangle", EliusArenaLoader.eliusArena.ToWorldRect());
				rectsRegistered = true;
			}
		}

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

		public static bool InAOSubworld
		{
			get
			{
				if (ModLoader.TryGetMod("SubworldLibrary", out Mod subworld) && (bool)subworld.Call("AnyActive", ArcaneOdysseyMod.Instance))
				{
					return true;
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

		public static void SetItemAttributes()
		{
			foreach (var item in ModContent.GetContent<ModItem>())
			{
				if (item.Mod.Name == "ThoriumMod")
				{
					switch (item.GetType().Namespace.Split('.')[^1])
					{
						case "Icy":
							ArcaneOdysseyMod.Sets.cold[item.Type] = true;
							break;
					}
				}

				string[] strength = [
					"ThoriumMod/TerrariansLastKnife",
					"ThoriumMod/WyvernSlayer",
					"ThoriumMod/QuakeGauntlet"
				];

				string[] arcanium = [

				];

				string[] artisinal = [
					"ThoriumMod/MastersLibram",
					"ThoriumMod/QuasarsFlare",
					"ThoriumMod/SnowWhite",
					"ThoriumMod/StellarSystem",
					"ThoriumMod/UselessStaff",
					"ThoriumMod/WondrousWand",
					"ThoriumMod/EclipseFang"
				];

				foreach (var strong in strength)
				{
					if (ModContent.TryFind<ModItem>(strong, out var theitem))
					{
						if (item.FullName == theitem.FullName)
						{
							ArcaneOdysseyMod.Sets.weaponType[item.Type] = WeaponType.Strength;
						}
					}
				}

				foreach (var artisan in artisinal)
				{
					if (ModContent.TryFind<ModItem>(artisan, out var theitem))
					{
						if (item.FullName == theitem.FullName)
						{
							ArcaneOdysseyMod.Sets.weaponType[item.Type] = WeaponType.Artisinal;
						}
					}
				}

				foreach (var arc in arcanium)
				{
					if (ModContent.TryFind<ModItem>(arc, out var theitem))
					{
						if (item.FullName == theitem.FullName)
						{
							ArcaneOdysseyMod.Sets.weaponType[item.Type] = WeaponType.Arcanium;
						}
					}
				}

				string[] cold = [
					"ThoriumMod/HydroPump",
					"ThoriumMod/TheWhirlpool",
					"ThoriumMod/Chum",
					"ThoriumMod/WhirlpoolSaber",
					"ThoriumMod/IcyGaze",
					"ThoriumMod/DeitysTrefork",
					"ThoriumMod/OceansJudgement",
					"ThoriumMod/SevenSeasDevastator",
					"ThoriumMod/TidalWave",
					"ThoriumMod/SeahorseWand",
					"ThoriumMod/BlobhornCoralStaff",
					"ThoriumMod/GeyserStaff",
					"ThoriumMod/SeaFoamScepter",
					"ThoriumMod/ClimbersIceAxe",
					"ThoriumMod/SpiritBreaker",
					"ThoriumMod/Freeze",
					"ThoriumMod/NitrogenVial",];

				string[] hot = [
					"ThoriumMod/TheSeaMine",
					"ThoriumMod/GodKiller",
					"ThoriumMod/AlmanacofAgony",
					"ThoriumMod/DevilsClaw",
					"ThoriumMod/EmberStaff",
					"ThoriumMod/PrometheanStaff",
					"ThoriumMod/DraconicMagmaStaff",
					"ThoriumMod/EruptingFlare",
					"ThoriumMod/EssenceofFlame",
					"ThoriumMod/GoldenLocks",
					"ThoriumMod/GolemsGaze",
					"ThoriumMod/HellishHalberd",
					"ThoriumMod/HellfireMinigun",
					"ThoriumMod/ObsidianStaff",
					"ThoriumMod/InfernalAnimator",
					"ThoriumMod/TheMassacre",
					"ThoriumMod/Ignite",
					"ThoriumMod/InfernoStaff",
					"ThoriumMod/DoomFireAxe",
					"ThoriumMod/SolScorchedSlab",
					"ThoriumMod/CinderString",
					"ThoriumMod/CometCrossfire",
					"ThoriumMod/MeteorHeadStaff",
					"ThoriumMod/CombustionFlask",
					"ThoriumMod/MoltenKnife",
					"ThoriumMod/MeteoriteClusterBomb",
					"ThoriumMod/PlasmaVial"];

				foreach (var colditem in cold)
				{
					if (ModContent.TryFind<ModItem>(colditem, out var theitem))
					{
						if (item.FullName == theitem.FullName)
						{
							ArcaneOdysseyMod.Sets.cold[item.Type] = false;
						}
					}
				}

				foreach (var hotitem in hot)
				{
					if (ModContent.TryFind<ModItem>(hotitem, out var theitem))
					{
						if (item.FullName == theitem.FullName)
						{
							ArcaneOdysseyMod.Sets.cold[item.Type] = true;
						}
					}
				}


				string[] greatswords = [
					"ThoriumMod/WyvernSlayer"
				];

				string[] greataxes = [
					"ThoriumMod/LodeStoneGreatAxe",
				];

				string[] daggers = [

				];

				string[] greathammer = [
					"ThoriumMod/MagicThorHammer",
					"ThoriumMod/RangedThorHammer",
					"ThoriumMod/MeleeThorHammer",
				];

				string[] spears = [
					"ThoriumMod/Spearmint"
				];

				string[] dualblades = [

				];

				string[] staffs = [

				];

				string[] rapiers = [
					"ThoriumMod/Rapier"
				];

				string[] claws = [
					"ThoriumMod/BloodyHighClaws"
				];


				foreach (var igotlazy in greatswords)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.greatsword[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in greataxes)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.greataxe[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in greathammer)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.greathammer[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in daggers)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.dagger[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in rapiers)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.rapier[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in dualblades)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.dualbladed[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in claws)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.claw[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in spears)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.spear[item.Type] = true;
						}
					}
				}


				foreach (var igotlazy in staffs)
				{
					if (ModContent.TryFind<ModItem>(igotlazy, out var theitem))
					{
						if (theitem.FullName == item.FullName)
						{
							ArcaneOdysseyMod.Sets.staff[item.Type] = true;
						}
					}
				}
			}
		}

		public static void CheckWeapon(ModItem item) { } // unused

		public override void Load()
		{
			if (ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				On_Dust.NewDust += DustScaleFixer;
				On_Dust.NewDustDirect += DirectDustScaleFixer;
				On_Dust.NewDustPerfect += PerfectDustScaleFixer;
			}

			if (HasFargos)
			{
				Fargos.Call("AddCaughtNPC", nameof(Edgelord), ModContent.NPCType<Edgelord>(), Name);
			}
		}

		private Dust PerfectDustScaleFixer(On_Dust.orig_NewDustPerfect orig, Vector2 Position, int Type, Vector2? Velocity, int Alpha, Color newColor, float Scale)
		{
			return orig(Position, Type, Velocity, Alpha, newColor, Scale.Clamp(1e-5f, 10f));
		}

		private Dust DirectDustScaleFixer(On_Dust.orig_NewDustDirect orig, Vector2 Position, int Width, int Height, int Type, float SpeedX, float SpeedY, int Alpha, Color newColor, float Scale)
		{
			return orig(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale.Clamp(1e-5f, 10f));
		}

		private int DustScaleFixer(On_Dust.orig_NewDust orig, Vector2 Position, int Width, int Height, int Type, float SpeedX, float SpeedY, int Alpha, Color newColor, float Scale)
		{
			return orig(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale.Clamp(1e-5f, 10f));
		}

		public override void Unload()
		{
			if (ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				On_Dust.NewDust -= DustScaleFixer;
				On_Dust.NewDustDirect -= DirectDustScaleFixer;
				On_Dust.NewDustPerfect -= PerfectDustScaleFixer;
			}
		}
	}

	//[ExtendsFromMod("MagicStorage")]
	//public class ImbuesFilter : FilteringOption
	//{
	//	public override ItemFilter.Filter Filter => item => item.ModItem is Imbuable or Scroll;

	//	public override string Texture => Mod.Name + "/Assets/ImbuesFilter";

	//	public override Position GetDefaultPosition() => new Between(FilteringOptionLoader.Definitions.Magic, FilteringOptionLoader.Definitions.Summon); // might not work till aqua finishes his update
	//}
}
