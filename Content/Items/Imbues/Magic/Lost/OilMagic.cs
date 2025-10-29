using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class OilMagic : AOMagic
	{
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(20,20,20);
        public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
        public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Oiled, 60*10)];
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			],
			[
				
			]
			);
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<OilBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<OilPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<OilCannon>())]);
		
		public override void AddRecipes() {
            
        }
	}
}