using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class JerminusMagic : AOMagic
	{
		public override Color ImbueColour => new Color(255,0,0,0);
		public override float AOImbueSpeed => 5f;
		public override float AOImbueSize => 10f;
		public override float AOImbueDamage => .01f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Custom;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Cursed, 10*60), new (ModContent.BuffType<Trauma>(), 10*60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<JerminusBlast>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<JerminusCannon>())]);
		
		public override void AddRecipes() {
            
        }
	}
}
