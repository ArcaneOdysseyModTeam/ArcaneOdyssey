using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcaneOdyssey.Content.Buffs.Stuns;
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
	public class MagmaMagic : AOMagic
    {
        public override bool? Cold => false;
        public override bool CanBeWet => false;
		public override Color ImbueColour => new(255,50,0,0);
		public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.975f;
		public override float AOScrollSpeed => 0.7f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.9f;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override AODebuffRequirement ImbueDebuff => new AODebuffRequirement(BuffID.OnFire3, 60*10);
		public override SynergyEffects Effects => new SynergyEffects(
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
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.075f)
			]
			);
			
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.InfernoFork, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 2.5f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile) 
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.InfernoFork, 0f, 0f, 0, default, 1.2f)];
			Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),1,1,DustID.SolarFlare,0f,0f,0,default,1.2f)];
			Lighting.AddLight(projectile.position,1f,0.19f,0f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.InfernoFork, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.SolarFlare, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1.4f)];
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.InfernoFork, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<MagmaBlast>()),]);
	}
}
