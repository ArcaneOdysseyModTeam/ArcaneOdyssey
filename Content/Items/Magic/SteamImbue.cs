using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class SteamImbue : AOMagic
	{
		public override float AOMagicDamage => .85f;
		public override float AOImbueDamage => .925f;
        public override float AOMagicSize => 1.15f;
        public override float AOImbueSize => 1.1f;
		public override float AOImbueSpeed => 1;
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];

		public AOMagic originalImbue;

		public override Color MagicColour => Color.LightGray;

		public override AOMagicTier MagicTier => AOMagicTier.Unobtainable;
		public override MagicEffects Effects => new([], [
			new(ModContent.BuffType<AOBleed>(), 1.15f),
			new(ModContent.BuffType<AOPetrified>(), 1.1f),
			new(BuffID.OnFire, 1.1f),
			new(ModContent.BuffType<CharredEffect>(), 1.1f),
			new(BuffID.Venom, 1.05f),
			new(ModContent.BuffType<FreezingEffect>(), .9f),
			new(BuffID.Wet, .9f),
			new(ModContent.BuffType<AOFrozen>(), .9f),
			new(ModContent.BuffType<Crystallized>(), .85f),
			new(ModContent.BuffType<SandyEffect>(), .8f),
			]);

		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), originalImbue.Spells.GetValueOrDefault(typeof(BlastSpell), ProjectileID.WoodenArrowFriendly))]);
	}
}