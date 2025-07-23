using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
    public class IceMagic : AOMagic
    {
        public override float AOImbueSpeed => .925f;
        public override float AOImbueSize => 1.15f;
        public override float AOImbueDamage => 1.05f;
        public override AODebuff? MagicDebuff => new(ModContent.BuffType<FreezingEffect>(), 60 * 10);
        public override AODebuff? MagicDebuff2 => new(BuffID.Chilled, 60, 33);
        public override CombinedDebuff[] combinedDebuffs => [new(BuffID.Wet, BuffID.Chilled)];

        public override MagicEffects Effects => new MagicEffects(
            [ // these are debuffs cleared on hit
                BuffID.Wet,
                BuffID.Bleeding,
                BuffID.Burning, 
                BuffID.Venom,
                BuffID.OnFire3,
            ],
            [ // synergies
                new MagicBuffMultiplier(BuffID.Bleeding, 1.2f), // bleeding
                new MagicBuffMultiplier(BuffID.Chilled, 1.1f), // frozen
                new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
                new MagicBuffMultiplier(BuffID.Wet, 1.1f), // (add stunning later!)
                new MagicBuffMultiplier(BuffID.OnFire, .9f), // burning
                new MagicBuffMultiplier(BuffID.Burning, .9f), // charred
                new MagicBuffMultiplier(BuffID.OnFire3, .8f), // scorched
            ]
            );
            public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
    }
}
