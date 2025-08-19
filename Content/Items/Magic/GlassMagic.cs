using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class GlassMagic : AOMagic
	{
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.1f;
		public override float AOMagicDamage => 0.9f;
		public override AODebuff? MagicDebuff => new AODebuff(ModContent.BuffType<AOBleed>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f)
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<GlassBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}