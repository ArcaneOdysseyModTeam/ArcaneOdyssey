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
	public class WindMagic : AOMagic
    {
        public override float DashSpeed => 1.5f; // instant
        public override float KBMulti => 2f;
        public override SoundStyle? ImbueSound => SoundID.Dig;
        public override Color ImbueColour => new(255,255,255,255);
		public override float AOImbueSpeed => 1.175f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => .9f;
		public override float AOScrollSpeed => 1.35f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => .825f;
        public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<SnowyEffect>(), ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];
		public override SynergyEffects Effects => new(
			[
				BuffID.OnFire,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				ModContent.BuffType<SandyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<AOScalding>(),
				BuffID.Oiled
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.9f),
				new MagicBuffMultiplier(BuffID.OnFire,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Poisoned,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.9f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.9f),
				new(BuffID.Oiled,0.98f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);
			public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n<3; n++)
			{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,DustID.BubbleBurst_White,projectile.velocity.X*2f,projectile.velocity.Y*2f,0,default,3f)];
					spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.BubbleBurst_White, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.BubbleBurst_White, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override List<Type> Skills => [typeof(WindBlast), typeof(WindPulsar), typeof(WindCannon)];
	}
}
