using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
    public class ExternalModSupport : ModSystem
    {
        public override void PostSetupContent()
        {
            MusicDisplaySetup();
			AddFargosStuff();
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

        private void MusicDisplaySetup()
        {
			if (ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay))
			{
				void AddMusic(string songName, string authorName, string songPath)
				{
					short slot = (short)MusicLoader.GetMusicSlot(Mod, $"Music/{songPath}");
					musicDisplay.Call("AddMusic", slot, songName, authorName, Mod.DisplayName);
				}
				AddMusic("The Call of Adventure", "Tobi", "TitleTheme");
				AddMusic("The Dark Sea", "Tobi", "DarkSea");
			}
        }

		private void AddFargosStuff()
		{
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos))
			{
				// stat sheet
				Func<string> SizeText = () => $"Attack size multiplier: {Math.Round(Main.LocalPlayer.ArcaneOdyssey().GetSizeMulti(), 3)}x";
				fargos.Call("AddStat", ModContent.ItemType<ColossalGreatsword>(), SizeText);

				// current imbue lol
				Func<string> imbueText = () => $"Current imbue: {(Main.LocalPlayer.ArcaneOdyssey().imbue is not null ? Main.LocalPlayer.ArcaneOdyssey().imbue.DisplayName : Mod.CustomLocalization("RandomWords.None"))}";
				fargos.Call("AddStat", ModContent.ItemType<PoseidonChoice>(), imbueText);

				fargos.Call("AddDevianttHelpDialogue", "Deviantt", (byte)2, (string _) => "No Conditions", "ArcaneOdyssey.NPCs.Edgelord");
			}
		}
	}
}
