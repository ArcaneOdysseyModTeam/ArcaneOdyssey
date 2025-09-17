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

namespace ArcaneOdyssey.Content.Items.Magic
{
    public class DeliriumMagic : AOMagic
    {
        public override Color MagicColour => new Color(255,255,255,0);
        public override float AOImbueSpeed => 2.3f;
        public override float AOImbueSize => 5f;
        public override float AOImbueDamage => .5f;
        public override float AOMagicDamage => 0.5f;
        public override float AOMagicSize => 5f;
        public override float AOMagicSpeed => 2.3f;
        public override AOMagicTier MagicTier => AOMagicTier.Custom;
        public override AODebuffRequirement MagicDebuff => new(BuffID.Confused, 10 * 60);
        public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
        public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<DeliriumBlast>()),]);
    }
}
