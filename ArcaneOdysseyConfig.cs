using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using System.Collections.Generic;

namespace ArcaneOdyssey
{
    public class ArcaneOdysseyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(false)]
        public bool AffectsOtherMods { get; set; }

        public List<string> IgnoredProjectiles { get; set; }


        public static ArcaneOdysseyConfig Instance;
    }
}