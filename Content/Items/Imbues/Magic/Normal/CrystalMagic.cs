using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
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
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class CrystalMagic : AOMagic
	{
		public override Color ImbueColour => new(255, 0, 0);
		public override float AOImbueSpeed => 0.95f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 1.025f;
		public override float AOScrollSpeed => 0.9f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 1.05f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<Crystallized>(), 60*5)];
		public override CombinedDebuff[] CombinedDebuffs => [];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f)
			]
			);
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<CrystalBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<CrystalPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<CrystalCannon>())]);

		public override void SpawningEffects(Entity projectile)
		{
            for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,DustID.GemRuby,projectile.velocity.X*0.4f,projectile.velocity.Y*0.4f,0,default,1f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.SilverFlame, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.Center, 1, 1, DustID.GemRuby, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f);
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.GemRuby, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 1f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}