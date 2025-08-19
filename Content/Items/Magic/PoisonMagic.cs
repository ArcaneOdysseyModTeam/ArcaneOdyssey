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
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PoisonMagic : AOMagic
	{
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 0.825f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.15f;
		public override float AOMagicDamage => 0.75f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.Poisoned, 60*10);
		//public override AODebuff? MagicDebuff2 => new AODebuff(BuffID.Stinky, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.075f),
				new MagicBuffMultiplier(BuffID.OnFire,0.99f)
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PoisonBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}