using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.Stuns;
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
	public class AshMagic : AOMagic
	{
		public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.95f;
		public override float AOMagicSpeed => 0.95f;
		public override float AOMagicSize => 1.25f;
		public override float AOMagicDamage => 0.875f;
		public override AODebuff? MagicDebuff => new AODebuff(ModContent.BuffType<AOPetrified>(), 60*10,33);
		public override CombinedDebuff[] combinedDebuffs => [new(BuffID.OnFire3, ModContent.BuffType<AOPetrified>()),new(BuffID.OnFire, ModContent.BuffType<AOPetrified>()),new(BuffID.ShadowFlame, ModContent.BuffType<AOPetrified>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
			public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}