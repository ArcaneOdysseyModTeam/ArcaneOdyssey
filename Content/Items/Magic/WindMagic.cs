using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
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

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class WindMagic : AOMagic
	{
		public override float AOImbueSpeed => 1.175f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => .9f;
		public override float AOMagicSpeed => 1.35f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => .825f;
        public override CombinedDebuff[] combinedDebuffs => [new(ModContent.BuffType<SnowyEffect>(), BuffID.Chilled), new(ModContent.BuffType<FreezingEffect>(), BuffID.Chilled)];
		public override MagicEffects Effects => new MagicEffects(
			[
				// sandy
				ModContent.BuffType<AOPetrified>(), // petrified
				BuffID.Poisoned,
				BuffID.OnFire,
				BuffID.Wet,
				BuffID.Burning,
			], 
			[
				new MagicBuffMultiplier(BuffID.Stoned, 1.2f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), 1.1f),
                new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.1f)
				// finish later lol
			]
			);
			public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<WindBlast>()),]);
public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}
