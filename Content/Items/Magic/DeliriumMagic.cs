using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Magic
{
    public class DeliriumMagic : AOMagic
    {
        public override Color ImbueColour => new Color(255,255,255,0);
        public override float AOImbueSpeed => 2.3f;
        public override float AOImbueSize => 5f;
        public override float AOImbueDamage => .5f;
        public override float AOScrollDamage => 0.5f;
        public override float AOScrollSize => 5f;
        public override float AOScrollSpeed => 2.3f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Custom;
        public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Confused, 10 * 60)];
        public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<DeliriumBlast>()),]);
    }
}
