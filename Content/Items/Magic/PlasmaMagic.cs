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

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PlasmaMagic : AOMagic
	{
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 0.948f;
		public override float AOImbueDamage => 0.9f;
		public override float AOMagicSpeed => 1.25f;
		public override float AOMagicSize => 1f;
		public override float AOMagicDamage => 0.825f;
		public override AODebuff? MagicDebuff => new AODebuff(BuffID.ShadowFlame, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
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