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

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
	public class IronLeg : FightingStyle
	{
		public override Color ImbueColour => Color.White;

		public override float AOImbueDamage => 1.125f;
		public override float AOImbueSpeed => 0.75f;
		public override float AOImbueSize => 1.1f;
		public override float AOScrollDamage => .95f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollSpeed => 0.75f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60*10)];
		public override SynergyEffects Effects => new(
			[
				ModContent.BuffType<FreezingEffect>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.2f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.1f)
			]
		);
	}
}
