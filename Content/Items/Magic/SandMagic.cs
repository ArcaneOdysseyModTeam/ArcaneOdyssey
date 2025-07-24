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
	public class SandMagic : AOMagic
	{
		public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1.05f;
		public override float AOMagicSpeed => 0.95f;
		public override float AOMagicSize => 1.1f;
		public override float AOMagicDamage => 0.975f;
		public override AODebuff? MagicDebuff => null;
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