using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
    public class IceMagic : AOMagic
    {
        public override float AOImbueSpeed => .925f;
        public override float AOImbueSize => 1.15f;
        public override float AOImbueDamage => 1.05f;
        public override float AOMagicSpeed => 0.85f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 0.975f;
        public override AODebuff? MagicDebuff => new(ModContent.BuffType<FreezingEffect>(), 60 * 10);
        public override AODebuff? MagicDebuff2 => new(ModContent.BuffType<AOFrozen>(), 60, 33);
        public override CombinedDebuff[] combinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>())];

        public override MagicEffects Effects => new MagicEffects(
            [ // these are debuffs cleared on hit
                BuffID.Wet,
                ModContent.BuffType<AOBleed>(),
                BuffID.Burning, 
                BuffID.Venom,
                BuffID.OnFire3,
                BuffID.ShadowFlame,
                ModContent.BuffType<CharredEffect>()
            ],
            [ // synergies
                new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
                new MagicBuffMultiplier(ModContent.BuffType<AOFrozen>(), 1.1f), // frozen
                new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
                new MagicBuffMultiplier(BuffID.Wet, 1.1f), // (add stunning later!)
                new MagicBuffMultiplier(BuffID.OnFire, .9f), // burning
                new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), .9f), // charred
                new MagicBuffMultiplier(BuffID.OnFire3, .8f), // scorched
                new MagicBuffMultiplier(BuffID.ShadowFlame, .8f),
                new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), 1.1f),
                new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),1.075f)
            ]
            );
            public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
    }
}
