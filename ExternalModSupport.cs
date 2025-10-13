using ArcaneOdyssey.Content.Items.Equipment.Scrolls;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.NPCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
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

			calamity.Call("CreateCodebreakerDialogOption", "Magic Pollution", "This is abundant with magic, to a scale of which has never been recorded even among stars. This may have not always been the case however, as the erotion patterns suggest the large amount of mana manifested a mere eight hundred years ago.", () => true);
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
