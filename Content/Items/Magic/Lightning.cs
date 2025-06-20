using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Items.Magic
{
    public class Lightning : AOMagic
    {
        public override float AOImbueSpeed => 1.2f;
        public override float AOImbueSize => .95f;
        public override float AOImbueDamage => .95f;
        public override AODebuff? MagicDebuff => new AODebuff(BuffID.Dazed, 60, 33);
        public override MagicEffects Effects => new MagicEffects(
            [
                BuffID.Stoned, 
                BuffID.Bleeding, 
                BuffID.Frozen
            ], 
            [
                new MagicBuffMultiplier(BuffID.Frozen, 1.2f),
                new MagicBuffMultiplier(BuffID.Bleeding, 1.2f),
                new MagicBuffMultiplier(BuffID.Burning, 1.15f),
                new MagicBuffMultiplier(BuffID.OnFire3, 1.075f),
                new MagicBuffMultiplier(BuffID.Venom, 1.075f),
                new MagicBuffMultiplier(BuffID.Wet, 1.05f)
            ]
            );
    }
}
