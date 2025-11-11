using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Other;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Other;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Other;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Other
{
	public class VesuviusMagic : AOMagic
	{
        public override Color ImbueColour => new(0, 0, 255);
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => 3f;
		public override float AOImbueDamage => 2f;
		public override float AOScrollSpeed => 1.2f;
		public override float AOScrollSize => 3f;
		public override float AOScrollDamage => 2f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
        public override SoundStyle? ImbueSound => SoundID.Item20;
        public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOPetrified>(),10*60), new AODebuffRequirement(BuffID.OnFire3,10*60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Chilled, // freezing
				ModContent.BuffType<AOPetrified>(),
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				BuffID.Venom,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SandyEffect>(),
				ModContent.BuffType<SnowyEffect>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOPetrified>(), 2.2f), // petrified
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 2.15f), // bleeding
				new MagicBuffMultiplier(BuffID.OnFire, 2.075f),
				new MagicBuffMultiplier(BuffID.Venom, 2.1f), // venom acid
				new MagicBuffMultiplier(BuffID.Burning, 2.075f),
				new MagicBuffMultiplier(BuffID.Poisoned, 2.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.95f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), 1.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), 2.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(), 1.99f),
				new MagicBuffMultiplier(BuffID.Wet, 1.95f),
				new MagicBuffMultiplier(BuffID.ShadowFlame, 2.1f),
				new MagicBuffMultiplier(BuffID.Slimed,2.075f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.95f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),2.075f)
			]
			);

		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<VesuviusBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<VesuviusPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<VesuviusCannon>())]);
		
        public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.UltraBrightTorch, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, new Color(0,0,255,0), 2.5f)];
				spawnedDust.noGravity = true;
			}
		}
		
		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.UltraBrightTorch, 0f, 0f, 0, new Color(0,0,255,0), 1.2f);
			Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.SolarFlare, 0f, 0f, 0, Color.Blue, 1.2f);
			Lighting.AddLight(projectile.position, 1f, 0.19f, 0f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.Center, 1, 1, DustID.UltraBrightTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, new Color(0,0,255,0), 2f);
				Dust.NewDust(projectile.Center, 1, 1, DustID.SolarFlare, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, Color.Blue, 2f);
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.UltraBrightTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, new Color(0, 0, 255, 0), 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}
