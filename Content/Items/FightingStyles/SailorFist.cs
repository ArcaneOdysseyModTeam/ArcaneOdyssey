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
using Terraria.Audio;
using ArcaneOdyssey.Content.Buffs.Stuns;

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
	public class SailorFist : FightingStyle
	{
		public override Color ImbueColour => Color.White;

		public override float AOImbueDamage => 0.925f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.278f;
		public override float AOScrollDamage => .85f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollSpeed => 1f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Wet, 60*10)];
		public override SynergyEffects Effects => new(
			[
				ModContent.BuffType<SearedEffect>(),
				ModContent.BuffType<CharredEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				BuffID.Venom,
				BuffID.ShadowFlame,
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<AOPetrified>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),0.9f),
				new MagicBuffMultiplier(BuffID.OnFire3,0.9f),
				new MagicBuffMultiplier(BuffID.Venom,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),0.85f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.OnFire,0.8f)
			]
		);
	}
}
