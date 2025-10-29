using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class DiamondMagic : AOMagic
    {
        public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
		public override Color ImbueColour => new(50,255,255);
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
        public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60*10)];
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.125f)
			]
			);
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<DiamondBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<DiamondPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<DiamondCannon>())]);
		
		public override void AddRecipes() {
            this.CreateLostRecipe(typeof(CrystalMagic), typeof(EarthMagic));
        }
	}
}