using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class MagmaMagic : AOMagic
    {
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.975f;
		public override float AOMagicSpeed => 0.7f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 0.9f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.OnFire3, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.Chilled, // freezing
				ModContent.BuffType<AOPetrified>(),
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				BuffID.Venom,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SandyEffect>(),
				ModContent.BuffType<SnowyEffect>()
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOPetrified>(), 1.2f), // petrified
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.15f), // bleeding
				new MagicBuffMultiplier(BuffID.OnFire, 1.075f),
				new MagicBuffMultiplier(BuffID.Venom, 1.1f), // venom acid
				new MagicBuffMultiplier(BuffID.Burning, 1.075f),
				new MagicBuffMultiplier(BuffID.Poisoned, 1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), .95f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), .99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(), 0.99f),
				new MagicBuffMultiplier(BuffID.Wet, .95f),
				new MagicBuffMultiplier(BuffID.ShadowFlame, 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.95f)
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<MagmaBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}
