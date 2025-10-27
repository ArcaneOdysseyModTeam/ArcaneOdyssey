using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.NPCS;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public static bool hasYapped = false;
		public override void PreUpdateWorld()
		{
			if (!(hasYapped || ModLoader.HasMod("ArcaneOdysseyMusic")))
			{
				hasYapped = true;
				Main.NewText("You are missing the Arcane Odyssey Music Mod (ArcaneOdysseyMusic). For the full experience, enable this mod.", Color.Teal);
			}
		}

		public static void RegisterDebuff(ModBuff buff)
		{
			if (ModLoader.TryGetMod("CalamityMod", out var cal))
			{
				cal.Call("RegisterDebuff", buff.Texture, (NPC e) => e.HasBuff(buff.Type));
			}
		}

		public static int GetMusic(string name, int fallback = 0)
		{
			if (ModLoader.TryGetMod("ArcaneOdysseyMusic", out Mod musicmod))
			{
				return (int)musicmod.Call(name);
			}
			else return fallback;
		}

		public void MiscCalamitysStuff()
		{
			if (!ModLoader.TryGetMod("CalamityMod", out Mod calamity))
				return;

            string[] descs = [Mod.CustomLocalization("CodebreakerDialogOption.Description1").Value, Mod.CustomLocalization("CodebreakerDialogOption.Description2").Value, Mod.CustomLocalization("CodebreakerDialogOption.Description3").Value, Mod.CustomLocalization("CodebreakerDialogOption.Description4").Value];
			calamity.Call("CreateCodebreakerDialogOption", Mod.CustomLocalization("CodebreakerDialogOption.Name").Value, 
                string.Join(' ', descs),
                () => true);
		}

		public static void DeclareMiniboss(int type)
		{
			if (!ModLoader.TryGetMod("CalamityMod", out Mod calamity))
				return;

			calamity.Call("DeclareMiniboss", type);
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
			if (ModLoader.HasMod("CalamityMod"))
			{
				return DashBind().GetAssignedKeys().Count == 0;
			}
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos))
			{
				return !(bool)fargos.Call("DoubleTapDashDisabled");
			}
			return true;
		}
		
		public static ModKeybind DashBind()
		{
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
			{
				var a = calamity.Code.GetType("CalamityMod.CalamityKeybinds");
				if (a is not null)
				{
					return (ModKeybind)a.GetProperty("DashHotkey").GetValue(null);
				}
			}
			else if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos))
			{
				var e = fargos.GetType().
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
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos))
			{
				// stat sheet
				Func<string> SizeText = () => $"Attack size multiplier: {1+Math.Round(Main.LocalPlayer.ArcaneOdyssey().GetSizeMulti(), 3)}x";
				fargos.Call("AddStat", ModContent.ItemType<SteamImbue>(), SizeText);

				// current imbue lol
				Func<string> imbueText = () => $"Current Imbue: {(Main.LocalPlayer.ArcaneOdyssey().Imbue is not null ? Main.LocalPlayer.ArcaneOdyssey().Imbue.DisplayName : Mod.CustomLocalization("RandomWords.None"))}";
				fargos.Call("AddStat", ModContent.ItemType<PoseidonChoice>(), imbueText);

				fargos.Call("AddDevianttHelpDialogue", "Deviantt", (byte)2, (string _) => "No Conditions", $"{nameof(ArcaneOdyssey)}.NPCs.Edgelord");
			}
		}

		public bool HasCalamity => ModLoader.HasMod("CalamityMod");
		public bool HasMusicMod => ModLoader.HasMod("ArcaneOdysseyMusic");
		public bool HasFargos => ModLoader.HasMod("Fargowiltas");

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
				//int spawnItem = ModContent.ItemType<>();
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
					//["spawnItems"] = spawnItem,
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
				if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
				{
					if (sick.HasValue)
						calamity.Call("SetVulnerabilities", NPC, "sick", sick.Value);
					if(electric.HasValue)
						calamity.Call("SetVulnerabilities", NPC, "electric", electric.Value);
					if (water.HasValue)
						calamity.Call("SetVulnerabilities", NPC, "water", water.Value);
					if (hot.HasValue)
						calamity.Call("SetVulnerabilities", NPC, "hot", hot.Value);
					if (cold.HasValue)
						calamity.Call("SetVulnerabilities", NPC, "cold", cold.Value);
				}
			}

			public static void SetDebuffVulnurablility(NPC NPC, bool? sick = null, bool? hot = null, bool? electric = null, bool? water = null, bool? cold = null) => new DebuffVulnurablilities(sick, hot, electric, water, cold).ApplyDebuffVulnurablility(NPC);
		}
	}
}
