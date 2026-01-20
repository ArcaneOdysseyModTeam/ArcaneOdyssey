using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class StormMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.5f; // instant
		public override float KBMulti => 1.25f;
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
		public override Color ImbueColour => Color.Gray;
		public override float AOImbueSpeed => 1.05f;
		public override float AOImbueSize => 1.265f;
		public override float AOImbueDamage => .95f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CloudyEffect>(), 3 * 60), new(ModContent.BuffType<AOParalyzed>(), 60, 16)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<SnowyEffect>(), ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];

		public override SynergyEffects Effects => new(
			[
				BuffID.OnFire,
				BuffID.Venom,
				ModContent.BuffType<SandyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<AOScalding>(),
				BuffID.Oiled,
				ModContent.BuffType<AOPetrified>(), // petrified
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<AOBleed>(),
			],
			[
				new(ModContent.BuffType<CloudyEffect>(), 1.1f),
				new(ModContent.BuffType<Crystallized>(),0.9f),
				new(BuffID.OnFire,0.9f),
				new(ModContent.BuffType<CharredEffect>(),1.125f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.Poisoned,0.9f),
				new(ModContent.BuffType<SandyEffect>(),0.9f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<AOScalding>(),0.9f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, Color.DimGray, 4f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.WitherLightning, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, 0f, 0f, 0, Color.DimGray, 1.5f)];
			spawnedDust.noGravity = true;
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.WitherLightning, 0f, 0f, 0, default, 0.75f);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, Color.DimGray, 4f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(projectile.Center, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize));
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Color.DimGray, 4f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.WitherLightning, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 1.2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(LightningMagic), typeof(WindMagic),typeof(WaterMagic),typeof(SnowMagic));
		}
	}
}
