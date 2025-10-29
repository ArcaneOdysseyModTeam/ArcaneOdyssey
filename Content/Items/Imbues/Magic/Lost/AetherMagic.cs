using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class AetherMagic : AOMagic
	{
		public override float AOImbueSpeed => 1f;
        public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
		
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<AetherBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<AetherPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<AetherCannon>())]);
		
		public override void AddRecipes() 
        {
            
        }
	}
}