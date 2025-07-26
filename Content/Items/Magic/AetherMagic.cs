using ArcaneOdyssey.Content.Items.Base;
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
	public class AetherMagic : AOMagic
	{
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1f;
		public override float AOMagicDamage => 1f;
		public override AODebuff? MagicDebuff => null;
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
			public override void AddRecipes() {
            
        }
	}
}