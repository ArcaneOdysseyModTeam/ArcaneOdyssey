using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Items.Materials;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PrismMagic : AOMagic
	{
        public override float AOImbueSpeed => 1f;
        public override float AOImbueDamage => 1f;
        public override float AOImbueSize => 1f;

        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
		
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PrismBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<PrismPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<PrismCannon>())]);
		
		public override void AddRecipes()
        {
			CreateRecipe().AddIngredient<HecateShard>().AddIngredient<LightMagic>().Register();
			CreateRecipe().AddIngredient<HecateShard>().AddIngredient<GlassMagic>().Register();
        }
	}
}