using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using System;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class StormMagic : MagicType
	{
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override bool ImmuneDash => true; // instant
		public override float KBMulti => 1.25f;
		public override SoundStyle? ImbueSound => SoundID.Thunder with { Volume = .6f }; // PORT change to InstantThunder
		public override Color ImbueColour => Color.DarkGray;
		public override Color ImbueColour2 => Color.Purple;
		public override bool AnimatedColours => true;
		public override float ScrollSpeed => 1.275f;
		public override float ScrollSize => 1.265f;
		public override float ScrollDamage => .95f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<CloudyEffect>(3 * 60), Debuff.Create<Paralyzed>(60, 16)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>(), Combo.Create<SnowyEffect, Frozen>(), Combo.Create<FreezingEffect, Frozen>()];


		public override MagicCircleTypes CircleType => MagicCircleTypes.Singularity;
		public override SynergyEffects Effects => new(
			[

				ClearBuff.Create<Burning>(),

				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<Petrified>(), // petrified
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Bleeding>(),
			],
			[
				Synergy.Create<CloudyEffect>(1.1f),
				Synergy.Create<Crystallized>(0.9f),

				Synergy.Create<Burning>(.9f),
				Synergy.Create<CharredEffect>(1.125f),
				Synergy.Create<FreezingEffect>(1.1f),

				Synergy.Create<Poisoned>(.9f),
				Synergy.Create<SandyEffect>(0.9f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<Scalding>(0.9f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override int BlastFrames => 6;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, direction.X * 2f, direction.Y * 2f, 0, Color.DimGray, 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, direction.X * 0.2f, direction.Y * 0.2f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 0f, 0f, 0, Color.DimGray, 1.5f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.75f * area.RelativeScale());
			var updates = (float)Main.GameUpdateCount;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				updates += projectile.numUpdates;
			}
			float waveVal = 10f * MathF.Abs(updates % 5f % 10f - 2.5f) - 12.5f;
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;

			Lighting.AddLight(area.Center(), 2, 1, 2);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), 0, Color.DimGray, 4f * intensity)];
				spawnedDust.noGravity = true;
				Dust.NewDustDirect(position, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 5; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.DimGray, 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
			}
			if (source is Projectile proj && Main.myPlayer == proj.owner)
				Projectile.NewProjectile(source.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, proj.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<WaterMagic>();
		}
	}
}
