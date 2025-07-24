using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Magic
{
    public class LightningMagic : AOMagic
    {
        public override float AOImbueSpeed => 1.2f;
        public override float AOImbueSize => .95f;
        public override float AOImbueDamage => .95f;
        public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1f;
		public override float AOMagicDamage => 1f;
        public override AODebuff? MagicDebuff => new AODebuff(ModContent.BuffType<AOParalyzed>(), 60, 33);
        public override CombinedDebuff[] combinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];
        public override MagicEffects Effects => new MagicEffects(
            [ // these are debuffs cleared on hit
                BuffID.Stoned, // petrified
                BuffID.Bleeding,
                BuffID.Frozen
            ], 
            [
                new MagicBuffMultiplier(BuffID.Chilled, 1.2f), // frozen
                new MagicBuffMultiplier(BuffID.Bleeding, 1.2f), // bleeding
                new MagicBuffMultiplier(BuffID.Burning, 1.15f), // scalding
                new MagicBuffMultiplier(BuffID.OnFire3, 1.075f), // melting/hellfire
                new MagicBuffMultiplier(BuffID.Venom, 1.075f), // venom acid
                new MagicBuffMultiplier(BuffID.Wet, 1.05f) // (add stunning later!)
            ]
            );
            public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
    }
}
