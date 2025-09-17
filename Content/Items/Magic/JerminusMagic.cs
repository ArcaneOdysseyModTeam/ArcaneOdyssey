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
	public class JerminusMagic : AOMagic
	{
		public override Color MagicColour => new Color(255,0,0,0);
		public override float AOImbueSpeed => 5f;
		public override float AOImbueSize => 10f;
		public override float AOImbueDamage => .01f;
        public override AOMagicTier MagicTier => AOMagicTier.Custom;
        public override AODebuffRequirement MagicDebuff => new(BuffID.Cursed, 10*60);
		public override AODebuffRequirement MagicDebuff2 => new(ModContent.BuffType<Trauma>(), 10*60);
		public override MagicEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<JerminusBlast>()),]);
		
		public override void AddRecipes() {
            
        }
	}
}
