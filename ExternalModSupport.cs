using ArcaneOdyssey.Content.Items.Armour.Vanity.Masks;
using ArcaneOdyssey.Content.Items.BossTrophies;
using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.Items.Weapons.Sunken;
using ArcaneOdyssey.Content.NPCS;
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
				Func<string> HasteStat = () => Mod.CustomLocalization("FargosSheet.CooldownMulti", Math.Round(100f * Main.LocalPlayer.ArcaneOdyssey().CooldownDurationMulti - 100f, 1)).Value;
				Fargos.Call("AddStat", ModContent.ItemType<SunkenSword>(), HasteStat);

				//Func<string> blood = () => Mod.CustomLocalization("FargosSheet.BloodDisease", Main.LocalPlayer.ArcaneOdyssey().BloodDiseaseName).Value;
				//Fargos.Call("AddStat", ItemID.PsychoKnife, blood);

				Fargos.Call("AddDevianttHelpDialogue", "Deviantt", (byte)2, (string _) => "No Conditions", $"{Mod.Name}.NPCs.{nameof(Edgelord)}");
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
				float weight = 7.1f; // right after wof
				Func<bool> downed = () => DownedBosses.downedEvander;
				int bossType = ModContent.NPCType<Evander>();
				int trophy = ModContent.ItemType<EvanderTrophy>();
				LocalizedText spawnInfo = Mod.CustomLocalization($"NPCs.{internalName}.SpawnInfo");

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
				float weight = 3.1f; // right after eow
				Func<bool> downed = () => DownedBosses.downedDusk;
				int bossType = ModContent.NPCType<Dusk>();
				//int trophy = ModContent.ItemType<DuskMask>();
				int mask = ModContent.ItemType<DuskMask>();
				LocalizedText spawnInfo = Mod.CustomLocalization($"NPCs.{internalName}.SpawnInfo");

				bossChecklist.Call(
				"LogMiniBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					["collectibles"] = new List<int> { mask },
					["spawnInfo"] = spawnInfo
				});
			}

			void LaelusStuff()
			{
				string internalName = nameof(Laelus);
				float weight = .5f; // right away!
				Func<bool> downed = () => DownedBosses.downedLaelus;
				int bossType = ModContent.NPCType<Laelus>();
				//int trophy = ModContent.ItemType<EvanderTrophy>();
				LocalizedText spawnInfo = Mod.CustomLocalization($"NPCs.{internalName}.SpawnInfo");

				bossChecklist.Call(
				"LogMiniBoss",
				Mod,
				internalName,
				weight,
				downed,
				bossType,
				new Dictionary<string, object>()
				{
					//["collectibles"] = new List<int> { trophy },
					["spawnInfo"] = spawnInfo
				});
			}

			EvanderStuff();
			DuskStuff();
			LaelusStuff();
		}
	}
}
