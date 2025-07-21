using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ArcaneOdyssey
{
    public class ArcaneOdysseyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        public bool AffectsOtherMods { get; set; }


        public static ArcaneOdysseyConfig Instance;
    }
}