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
using ArcaneOdyssey.Content.Buffs.Stuns;

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
	public class SailorStyle : FightingStyle
	{
		public override bool? Cold => true;
		public override Color ImbueColour => Color.Aqua;
		public override SoundStyle? ImbueSound => SoundID.Splash;

		public override float AOImbueDamage => 0.925f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.278f;
		public override float AOScrollDamage => .85f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollSpeed => 1f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Wet, 60 * 10)];
		public override SynergyEffects Effects => new(
			[
				ModContent.BuffType<SearedEffect>(),
				ModContent.BuffType<CharredEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				BuffID.Venom,
				BuffID.ShadowFlame,
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<AOPetrified>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),0.9f),
				new MagicBuffMultiplier(BuffID.OnFire3,0.9f),
				new MagicBuffMultiplier(BuffID.Venom,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),0.85f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.OnFire,0.8f)
			]
		);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Water, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Water, 0f, 0f, 0, default, 1.2f)];
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Water, (8f * (Main.rand.NextFloat() - 0.5f)), (8f * (Main.rand.NextFloat() - 0.5f)), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Coral,15).Register();
		}
	}
}
