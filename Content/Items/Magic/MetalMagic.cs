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
	public class MetalMagic : AOMagic
	{
		public override float AOImbueSpeed => 0.825f;
		public override float AOImbueSize => 1.158f;
		public override float AOImbueDamage => 1.1f;
		public override float AOMagicSpeed => 0.65f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 1.025f;
		public override AODebuff? MagicDebuff => new AODebuff(ModContent.BuffType<AOBleed>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>()
			], 
			[
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.02f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f)

			]
			);
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<MetalBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}