using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.VFX.Dusts;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class GravityMagic : AOMagic
	{
		public override SoundStyle? ImbueSound => SoundID.NPCHit52;
		public override Color ImbueColour => new Color(120, 0, 200, 255);
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1.1f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60*10)];
		
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(BuffID.OnFire,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Poisoned,0.9f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.9f),
				new MagicBuffMultiplier(BuffID.Oiled,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f)
			]
			);
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<GravityBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<GravityPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<GravityCannon>())]);
        public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n<3; n++)
			{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,ModContent.DustType<GravityDust>(),projectile.velocity.X*0.5f,projectile.velocity.Y*0.5f,0,default,2f)];
					spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, ModContent.DustType<GravityDust>(), 0f, 0f, 0, default, 2.3f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, ModContent.DustType<GravityDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, ModContent.DustType<GravityDust>(), 8f * Main.rand.NextFloat() - 0.5f, 8f * Main.rand.NextFloat() - 0.5f, 0, default, 4f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override void AddRecipes()
        {
            CreateLostRecipe(typeof(EarthMagic), typeof(WindMagic), typeof(MagmaMagic));
        }
	}
}