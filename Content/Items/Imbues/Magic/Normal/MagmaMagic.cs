using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Normal;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class MagmaMagic : AOMagic
    {
        public override float DashResist => 1.2f;
        public override bool? Cold => false;
        public override bool CanBeWet => false;
        public override Color ImbueColour => new(255, 50, 0);
		public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.975f;
		public override float AOScrollSpeed => 0.7f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.9f;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.OnFire3, 60*10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Chilled, // freezing
				ModContent.BuffType<AOPetrified>(),
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				BuffID.Venom,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SandyEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Oiled
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOPetrified>(), 1.2f), // petrified
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.15f), // bleeding
				new MagicBuffMultiplier(BuffID.OnFire, 1.075f),
				new MagicBuffMultiplier(BuffID.Venom, 1.1f), // venom acid
				new MagicBuffMultiplier(BuffID.Burning, 1.075f),
				new MagicBuffMultiplier(BuffID.Poisoned, 1.05f),
				new MagicBuffMultiplier(BuffID.Slimed,1.075f),
				new MagicBuffMultiplier(BuffID.Oiled,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), .95f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), .99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(), 0.99f),
				new MagicBuffMultiplier(BuffID.Wet, .95f),
				new MagicBuffMultiplier(BuffID.ShadowFlame, 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);
			
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.InfernoFork, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 2.5f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile) 
		{
			Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.InfernoFork, 0f, 0f, 0, default, 1.2f);
			Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),1,1,DustID.SolarFlare,0f,0f,0,default,1.2f);
			Lighting.AddLight(projectile.position,1f,0.19f,0f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.InfernoFork, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(projectile.Center, 1, 1, DustID.SolarFlare, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1.4f);
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.InfernoFork, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

		public override List<Type> Skills => [typeof(MagmaBlast), typeof(MagmaPulsar), typeof(MagmaCannon)];
	}
}
