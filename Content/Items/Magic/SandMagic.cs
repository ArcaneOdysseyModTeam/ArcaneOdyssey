using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class SandMagic : AOMagic
    {
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1.05f;
		public override float AOMagicSpeed => 0.95f;
		public override float AOMagicSize => 1.1f;
		public override float AOMagicDamage => 0.975f;
		public override AODebuff? MagicDebuff => new AODebuff(ModContent.BuffType<SandyEffect>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.Wet
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire,1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.8f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(BuffID.Wet,0.8f)
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<SandBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}