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
using ArcaneOdyssey.Content.Buffs.MagicMarks;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class AetherMagic : AOMagic
	{
		public override Color ImbueColour => new(255, 255, 0, 255);
		public override bool? Cold => false;
        public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1.28f;
        public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => 1.15f;
		public override float AOScrollSpeed => 1.28f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.15f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CharredEffect>(), 60*10),new(ModContent.BuffType<BlindedEffect>(), 60*5)];
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