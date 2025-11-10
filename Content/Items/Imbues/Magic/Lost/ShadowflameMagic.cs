using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;


namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class ShadowflameMagic : AOMagic
	{
        public override bool? Cold => false;
        public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new Color(255, 100, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
        public override bool CanBeWet => true;
        public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1.1f;
		public override float AOScrollSpeed => 1.1f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 1.05f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.ShadowFlame, 60*10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<CharredEffect>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.Wet,0.99f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(BuffID.Poisoned,1.05f),
				new MagicBuffMultiplier(BuffID.OnFire,1.1f),
				new MagicBuffMultiplier(BuffID.Slimed,1.075f),
				new MagicBuffMultiplier(BuffID.Oiled,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.98f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.1f)
				
			]
			);

		public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.FireworkFountain_Pink, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 1f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Shadowflame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2.4f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.Shadowflame, 0f, 0f, 0, default, 1.6f);
			Dust spawnedDust = Dust.NewDustDirect(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.FireworkFountain_Pink, 0f, 0f, 0, default, 0.8f);
			spawnedDust.noGravity = true;
        }
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.FireworkFountain_Pink, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1.3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Shadowflame, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2.8f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.FireworkFountain_Pink, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Shadowflame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2.8f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<ShadowflameBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<ShadowflamePulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<ShadowflameCannon>())]);
		
		public override void AddRecipes() 
        {
            CreateLostRecipe(typeof(ShadowMagic), typeof(FireMagic),typeof(PlasmaMagic));
        }
	}
}