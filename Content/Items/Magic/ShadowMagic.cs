using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
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
using ArcaneOdyssey.Content.Items.Materials;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class ShadowMagic : AOMagic
	{
		public override float AOImbueSpeed => 1.125f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1.025f;
		public override float AOMagicSpeed => 1.25f;
		public override float AOMagicSize => 1.1f;
		public override float AOMagicDamage => 0.95f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.Obstructed, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.7f),
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<ShadowBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}