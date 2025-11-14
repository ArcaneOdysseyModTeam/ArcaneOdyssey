using ArcaneOdyssey.Content.Buffs.Stuns;
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
	public class JerminusMagic : AOMagic
	{
        public override Color ImbueColour => new(255, 0, 0);
		public override float AOImbueSpeed => 5f;
		public override float AOImbueSize => 10f;
        public override float DashStat => 2f;
		public override float AOImbueDamage => .2f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
        public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Cursed, 10 * 60), new(ModContent.BuffType<Trauma>(), 10 * 60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
		public override List<Type> Skills => [typeof(JerminusBlast), typeof(JerminusPulsar), typeof(JerminusCannon)];
	}
}
