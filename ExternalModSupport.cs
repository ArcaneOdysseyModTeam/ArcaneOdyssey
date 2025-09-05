using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
    public class ExternalModSupport : ModSystem
    {
        public override void PostSetupContent()
        {
            MusicDisplaySetup();
        }

        private void MusicDisplaySetup()
        {
            if (!ModLoader.TryGetMod("MusicDisplay", out Mod musicDisplay))
                return;

            void AddMusic(string songName, string authorName, string songPath)
            {
                short slot = (short)MusicLoader.GetMusicSlot(Mod, $"Music/{songPath}");
                musicDisplay.Call("AddMusic", slot, songName, authorName, Mod.DisplayName);
            }
            AddMusic("The Call of Adventure", "Tobi", "TitleTheme");
            AddMusic("The Dark Sea", "Tobi", "DarkSea");
        }
    }
}
