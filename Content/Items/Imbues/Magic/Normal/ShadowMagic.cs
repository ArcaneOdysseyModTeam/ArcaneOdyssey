using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class ShadowMagic : AOMagic
	{
		public override SoundStyle? ImbueSound => SoundID.Item8;
        public override Color ImbueColour => new(0,0,0,255);
		public override float AOImbueSpeed => 1.125f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1.025f;
		public override float AOScrollSpeed => 1.25f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 0.95f;
		public override AODebuffRequirement[] ImbueDebuffs => [new AODebuffRequirement(ModContent.BuffType<DrainedEffect>(), 60*10)];
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<BlindedEffect>(),0.7f),
				new MagicBuffMultiplier(BuffID.Confused,0.7f)
			]
			);
			public override void SpawningEffects(Entity projectile) 
			{
				for (int n = 0; n<3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,DustID.Wraith,projectile.velocity.X*2f,projectile.velocity.Y*2f,0,default,3f)];
					spawnedDust.noGravity = true;
				}
			}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.Wraith, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Wraith, 8f * Main.rand.NextFloat() - 0.5f, 8f * Main.rand.NextFloat() - 0.5f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<ShadowBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<ShadowPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<ShadowCannon>())]);
	}
}