using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
    public abstract class FightingStyle : Imbuable, ILocalizedModType
    {
        public override string LocalizationCategory => "FightingStyles"; // this is all lmao
    }
}
