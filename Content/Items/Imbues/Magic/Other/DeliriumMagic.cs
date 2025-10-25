using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Other;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Other;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Other;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Other
{
    public class DeliriumMagic : AOMagic
    {
        public override Color ImbueColour => new(255,255,255,0);
        public override float AOImbueSpeed => 2.3f;
        public override float AOImbueSize => 5f;
        public override float AOImbueDamage => .5f;
        public override float AOScrollDamage => 0.5f;
        public override float AOScrollSize => 5f;
        public override float AOScrollSpeed => 2.3f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
        public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Confused, 10 * 60)];
        public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<DeliriumBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<DeliriumPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<DeliriumCannon>())]);
    }
}
