using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
	public class ThermoFist : FightingStyle
	{
		public override Color ImbueColour => Color.White;

		public override float AOImbueDamage => 0.85f;
		public override float AOImbueSpeed => 1.3f;
		public override float AOImbueSize => 0.833f;
		public override float AOScrollDamage => .75f;
		public override float AOScrollSize => 1.3f;
		public override float AOScrollSpeed => 0.8f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SearedEffect>(), 60*10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.OnFire,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.1f),
			]
		);
	}
}
