using ArcaneOdyssey.Content.Items.Base;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Projectiles.Magic.MagicEffects;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class AetherMagic : AOMagic
	{
		public override float DashSpeed => 1.5f; // instant
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override Color ImbueColour => new(255, 255, 0, 255);
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1.28f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => 1.15f;
		public override float AOScrollSpeed => 1.28f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.15f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
        public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CharredEffect>(), 60 * 10), new(ModContent.BuffType<BlindedEffect>(), 60 * 5)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.OnFire,1.125f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.99f),
				new(BuffID.Oiled,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<DrainedEffect>(),0.8f)
			]
			);


		public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.YellowStarDust, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,DustID.YellowTorch,projectile.velocity.X*0.2f,projectile.velocity.Y*0.2f,0,default,3f)];
				spawnedDust2.noGravity = true;
			}
			}

		public override void LingeringEffects(Entity projectile)
		{
			if (!Main.dedServ) 
			{ 
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.YellowStarDust, 0f, 0f, 0, default, 1f);
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.YellowTorch, 0f, 0f, 0, default, 2f)];
				spawnedDust.noGravity = true;
				Lighting.AddLight(projectile.position, 2, 2, 0); 
			}
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.YellowTorch, (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
			if (projectile is Projectile proj && proj.ModProjectile is not AetherExplosion)
			{
				if (proj.owner == Main.myPlayer && AetherExplosion.Count < 1)
				{
					Projectile.NewProjectile(proj.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), proj.damage / 4, 0, proj.owner);
				}
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.YellowStarDust, 28f * (Main.rand.NextFloat() - 0.5f), 28f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.YellowTorch, 28f * (Main.rand.NextFloat() - 0.5f), 28f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
			if (projectile is Projectile proj && proj.ModProjectile is not AetherExplosion)
			{
				if (proj.owner == Main.myPlayer && AetherExplosion.Count < 3)
				{
					Projectile.NewProjectile(proj.GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<AetherExplosion>(), proj.damage / 4, 0, proj.owner);
				}
			}
		}

		public override void AddRecipes() 
		{
			CreateLostRecipe(typeof(LightMagic), typeof(PlasmaMagic),typeof(ExplosionMagic),typeof(AshMagic),typeof(FireMagic));
		}
	}
}
