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
	public class MagmaMagic : AOMagic
	{
		public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.975f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1f;
		public override float AOMagicDamage => 1f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.OnFire3, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.Chilled, // freezing
				BuffID.Stoned,
				BuffID.Wet,
				BuffID.Bleeding,
				BuffID.Venom,
				BuffID.Frozen
			], 
			[
				new MagicBuffMultiplier(BuffID.Stoned, 1.2f), // petrified
				new MagicBuffMultiplier(BuffID.Bleeding, 1.15f), // bleeding
				new MagicBuffMultiplier(BuffID.OnFire, 1.10f),
				new MagicBuffMultiplier(BuffID.Venom, 1.1f), // venom acid
				new MagicBuffMultiplier(BuffID.Burning, 1.075f),
				new MagicBuffMultiplier(BuffID.Poisoned, 1.05f),
				new MagicBuffMultiplier(BuffID.Chilled, .95f),
				new MagicBuffMultiplier(BuffID.Wet, .95f)
			]
			);
			public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}
