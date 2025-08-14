using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
using ArcaneOdyssey.Content.Items.Materials;

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
		public override CombinedDebuff[] combinedDebuffs => [new(BuffID.OnFire3, ModContent.BuffType<AOPetrified>()),new(BuffID.OnFire, ModContent.BuffType<AOPetrified>()),new(BuffID.ShadowFlame, ModContent.BuffType<AOPetrified>()),new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				ModContent.BuffType<CharredEffect>(),
				BuffID.ShadowFlame
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire,1.02f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.995f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f)
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<AshBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}