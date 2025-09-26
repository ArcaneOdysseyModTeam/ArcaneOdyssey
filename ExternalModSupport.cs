using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
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
			AddFargosStats();
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

		private void AddFargosStats()
		{
			if (ModLoader.TryGetMod("Fargowiltas", out Mod fargos))
			{
				// stat sheet
				Func<string> SizeText = () => $"Attack size multiplier: {Math.Round(Main.LocalPlayer.ArcaneOdyssey().GetSizeMulti(), 3)}x";
				fargos.Call("AddStat", ModContent.ItemType<ColossalGreatsword>(), SizeText);

				// current imbue lol
				Func<string> imbueText = () => $"Current imbue: {Main.LocalPlayer.ArcaneOdyssey().imbue.DisplayName}";
				fargos.Call("AddStat", ModContent.ItemType<PoseidonChoice>(), imbueText);
			}
		}
    }
}
