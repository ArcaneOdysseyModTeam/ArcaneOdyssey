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

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class AcidMagic : AOMagic
	{
		public override Color MagicColour => new Color(245,0,240,0);
		public override float AOImbueSpeed => 0.925f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.05f;
		public override float AOMagicDamage => 0.875f;
        public override SoundStyle? MagicSound => SoundID.Splash;
		public override AODebuffRequirement MagicDebuff => new(BuffID.Venom, 60*10);
		public override MagicEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<SandyEffect>()
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.075f),
				new MagicBuffMultiplier(BuffID.OnFire,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.2f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(BuffID.Poisoned,1.05f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f),
				new MagicBuffMultiplier(BuffID.Wet,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.99f)
			]
			);
			public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<AcidBlast>()),]);
			public override void SpawningEffects(Projectile projectile) 
			{
				for (int n = 0; n<3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.UnholyWater,(projectile.velocity.X*2f),(projectile.velocity.Y*2f),0,default,3f)];
					spawnedDust.noGravity = true;
				}
			}
			public override void LingeringEffects(Projectile projectile)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Venom, 0f, 0f, 0, default, 1f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),1,1,DustID.UnholyWater,0f,0f,0,default,1.6f)];
			}
		public override void ExplosionEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Venom, (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), 0, default, 1f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.UnholyWater, (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), 0, default, 3f)];
			}
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.UnholyWater, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
	}
}