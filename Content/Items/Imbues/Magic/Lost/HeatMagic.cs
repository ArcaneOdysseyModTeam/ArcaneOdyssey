using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;
using Terraria;
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class HeatMagic : AOMagic
	{
		public override Color ImbueColour => new(255,0,0,255);
        public override bool? Cold => false;
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SearedEffect>(), 60*10)];
        
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
				public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<HeatBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<HeatPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<HeatCannon>())]);
		
		public override void AddRecipes() {
            
        }
	}
}