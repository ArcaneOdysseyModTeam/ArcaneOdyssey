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

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class CrystalMagic : AOMagic
	{
		public override float AOImbueSpeed => 0.95f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 1.025f;
		public override float AOMagicSpeed => 0.9f;
		public override float AOMagicSize => 1.15f;
		public override float AOMagicDamage => 1.05f;
		public override AODebuff? MagicDebuff => new AODebuff(ModContent.BuffType<CrystalStackI>(), 60*10);
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CrystalStackIII>(),ModContent.BuffType<CrystalStackIIII>()),new(ModContent.BuffType<CrystalStackII>(),ModContent.BuffType<CrystalStackMid>()),new(ModContent.BuffType<CrystalStackI>(),ModContent.BuffType<CrystalStackII>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),1.3f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f)
			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<CrystalBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}