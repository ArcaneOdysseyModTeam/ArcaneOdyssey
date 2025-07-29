using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
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
	public class FireMagic : AOMagic
	{
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 0.925f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.15f;
		public override float AOMagicDamage => 0.85f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.OnFire, 60*10);
		public override CombinedDebuff[] combinedDebuffs => [new (ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.Wet,0.99f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(BuffID.Poisoned,1.05f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f)
				
			]
			);
			public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}