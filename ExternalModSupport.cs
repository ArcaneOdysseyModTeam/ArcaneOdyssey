using ArcaneOdyssey.Content.Items.BossTrophies;
using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.NPCS;
using Microsoft.Xna.Framework;
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

		internal static bool hasYapped = false;
		public override void PreUpdateWorld()
		{
			if (!(hasYapped || HasMusicMod))
			{
				hasYapped = true;
				Main.NewText("You are missing the Arcane Odyssey Music Mod (ArcaneOdysseyMusic). For the full experience, enable this mod.", Color.Teal);
			}
		}

		public static void RegisterDebuff(ModBuff buff)
		{
			if (HasCalamity)
			{
				Calamity.Call("RegisterDebuff", buff.Texture, (NPC e) => e.HasBuff(buff.Type));
			}
		}

		public static int GetMusic(string name, int fallback = 0)
		{
			if (HasMusicMod)
			{
				return (int)MusicMod.Call(name);
			}
			else return fallback;
		}

		public void MiscCalamitysStuff()
		{
			if (!HasCalamity)
				return;

			string[] descs = [Mod.CustomLocalization("CodebreakerDialogOption.Description1").Value, Mod.CustomLocalization("CodebreakerDialogOption.Description2").Value, Mod.CustomLocalization("CodebreakerDialogOption.Description3").Value, Mod.CustomLocalization("CodebreakerDialogOption.Description4").Value];
			string[] descs2 = [Mod.CustomLocalization("CodebreakerDialogOption.DemiDescription1").Value, Mod.CustomLocalization("CodebreakerDialogOption.DemiDescription2").Value, Mod.CustomLocalization("CodebreakerDialogOption.DemiDescription3").Value, Mod.CustomLocalization("CodebreakerDialogOption.DemiDescription4").Value];
			Calamity.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.Name").Value, 
				string.Join(' ', descs),
				() => !ArcaneOdysseyMod.DevMode);
			Calamity.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.Name").Value,
				string.Join(' ', descs2),
				() => ArcaneOdysseyMod.DevMode);
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

		public static void SetCalamityDash(string ID, Player player, bool force = false)
		{
			//if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
			//{
			//	if (calamity.TryFind("CalamityPlayer", out ModPlayer modPlayer))
			//	{
			//		foreach (ModPlayer pleyer in player.ModPlayers)
			//		{
			//			if (pleyer.GetType().Name == "CalamityPlayer")
			//			{
			//				var dashid = modPlayer.GetType().GetProperty("DashID");
			//				if (force || (dashid.GetValue(pleyer) is not null && (string)dashid.GetValue(pleyer) == "Default Dash"))
			//					dashid.SetValue(pleyer, ID);
			//				return;
			//			}
			//		}
			//	}
			//}
		}

		private void AddFargosStuff()
		{
			if (HasFargos)
			{
				// stat sheet
				Func<string> SizeText = () => $"Attack size multiplier: {1 + Math.Round(Main.LocalPlayer.ArcaneOdyssey().SizeMulti, 3)}x";
				Fargos.Call("AddStat", ModContent.ItemType<ColossalGreatsword>(), SizeText);

				// current imbue lol
				Func<string> imbueText = () => $"Current Imbue: {(Main.LocalPlayer.ArcaneOdyssey().Imbue is not null ? Main.LocalPlayer.ArcaneOdyssey().Imbue.DisplayName : Mod.CustomLocalization("RandomWords.None"))}";
				Fargos.Call("AddStat", ModContent.ItemType<PoseidonChoice>(), imbueText);

				Fargos.Call("AddDevianttHelpDialogue", "Deviantt", (byte)2, (string _) => "No Conditions", $"{Mod.Name}.NPCs.Edgelord");
			}
		}

		public static bool HasCalamity => ModLoader.HasMod("CalamityMod");
		public static Mod Calamity => ModLoader.GetMod("CalamityMod");
		public static bool HasMusicMod => ModLoader.HasMod("ArcaneOdysseyMusic");
		public static Mod MusicMod => ModLoader.GetMod("ArcaneOdysseyMusic");
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
				LocalizedText spawnInfo = Mod.CustomLocalization("NPCs.Evander.SpawnInfo");

				bossChecklist.Call(
				"LogBoss",
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

			EvanderStuff();
		}

		public struct DebuffVulnurablilities(bool? sick = null, bool? hot = null, bool? electric = null, bool? water = null, bool? cold = null)
		{
			public bool? sick = sick;
			public bool? hot = hot;
			public bool? electric = electric;
			public bool? water = water;
			public bool? cold = cold;

			public readonly void ApplyDebuffVulnurablility(NPC NPC)
			{
				if (HasCalamity)
				{
					if (sick.HasValue)
						Calamity.Call("SetVulnerabilities", NPC, "sick", sick.Value);
					if (electric.HasValue)
						Calamity.Call("SetVulnerabilities", NPC, "electric", electric.Value);
					if (water.HasValue)
						Calamity.Call("SetVulnerabilities", NPC, "water", water.Value);
					if (hot.HasValue)
						Calamity.Call("SetVulnerabilities", NPC, "hot", hot.Value);
					if (cold.HasValue)
						Calamity.Call("SetVulnerabilities", NPC, "cold", cold.Value);
				}
			}
			public static void SetDebuffVulnurablility(NPC NPC, bool? sick = null, bool? hot = null, bool? electric = null, bool? water = null, bool? cold = null) => new DebuffVulnurablilities(sick, hot, electric, water, cold).ApplyDebuffVulnurablility(NPC);
		}
	}
}
