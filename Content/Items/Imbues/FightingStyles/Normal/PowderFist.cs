using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker.Effects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class PowderFist : FightingStyle
	{
		public override float Aura => .875f;
		public override float DashSpeed => 1.2f;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override Color ImbueColour => Color.DarkGray;
		public override SoundStyle? ImbueSound => SoundID.Item14;

		public override float AOImbueDamage => Main.rand.NextFloat(0.85f, 1.17f);
		public override float AOImbueSpeed => .9f;
		public override float AOImbueSize => Main.rand.NextFloat(1.11f, 1.25f);
		public override float AOScrollDamage => Main.rand.NextFloat(0.7f, .96f);
		public override float AOScrollSize => Main.rand.NextFloat(1.05f, 1.19f);
		public override float AOScrollSpeed => .9f;

		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.OnFire, ModContent.BuffType<AOPetrified>()), new(ModContent.BuffType<AOScalding>(), ModContent.BuffType<AOPetrified>()), new(ModContent.BuffType<SearedEffect>(), ModContent.BuffType<AOPetrified>())];

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CharredEffect>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[
				BuffID.Wet,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<AOFrozen>(),
				ModContent.BuffType<AOParalyzed>(),
				ModContent.BuffType<AOPetrified>(),
				ModContent.BuffType<AOScalding>(),
				BuffID.OnFire,
				ModContent.BuffType<SearedEffect>(),
			],
			[
				new(ModContent.BuffType<AOPetrified>(), 1.1f),
				new(ModContent.BuffType<AOScalding>(), 1.1f),
				new(BuffID.OnFire, 1.1f),
				new(ModContent.BuffType<SearedEffect>(), 1.1f),
				new(BuffID.Venom, 1.1f),
				new(ModContent.BuffType<SandyEffect>(), 1.1f),
				new(BuffID.OnFire3, 1.1f),
				new(BuffID.Wet, .9f),
				new(ModContent.BuffType<FreezingEffect>(), .9f),
				new(ModContent.BuffType<SnowyEffect>(), .8f),
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, Scale: 1.6f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, Scale: 1.6f * area.RelativeScale())];
			spawnedDust3.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 4f * intensity)];
				spawnedDust3.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f)];
				spawnedDust2.noGravity = true;
			}
			if (source is Projectile projectile && projectile.ModProjectile is not PowderExplosion)
			{
				Projectile.NewProjectile(projectile.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), projectile.damage / 2, 3f, projectile.owner);
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.ExplosivePowder, 15).Register();
		}
	}
}
