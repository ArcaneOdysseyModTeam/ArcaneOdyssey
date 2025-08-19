using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
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
	public class PlasmaMagic : AOMagic
    {
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 0.948f;
		public override float AOImbueDamage => 0.9f;
		public override float AOMagicSpeed => 1.25f;
		public override float AOMagicSize => 1f;
		public override float AOMagicDamage => 0.825f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.ShadowFlame, 60*10);
		public override CombinedDebuff[] CombinedDebuffs => [new (ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(BuffID.OnFire,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.97f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(BuffID.Poisoned,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.Wet,0.95f)
			]
			);
		public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PlasmaBlast>()),]);
		
		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<HecateOrb>(1);
			recipe.Register();
		}
	}
}