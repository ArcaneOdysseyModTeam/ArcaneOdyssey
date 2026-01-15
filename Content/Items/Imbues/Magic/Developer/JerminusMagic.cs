using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Developer
{
	public class JerminusMagic : AOMagic
	{
		public override Color ImbueColour => new(255, 0, 0);
		public override float AOScrollSpeed => 5f;
		public override float AOScrollSize => 10f;
		public override float DashSpeed => 2f;
		public override float AOScrollDamage => .2f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Cursed, 10 * 60), new(ModContent.BuffType<Trauma>(), 10 * 60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[

			]
			);
	}
}
