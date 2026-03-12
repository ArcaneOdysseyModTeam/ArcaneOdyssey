using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class StormMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.4f; // instant
		public override float KBMulti => 1.25f;
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
		public override Color ImbueColour => Color.Lerp(Color.Purple, Color.DarkGray, Math.Abs(MathF.Tan(AOUtils.UpdateCount))); // lerp between purple and gray quickly, more commonly gray
		public override float AOImbueSpeed => 1.05f;
		public override float AOImbueSize => 1.265f;
		public override float AOImbueDamage => .95f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<CloudyEffect>(3 * 60), Debuff.Create<Paralyzed>(60, 16)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>(), Combo.Create<SnowyEffect, AOFrozen>(), Combo.Create<FreezingEffect, AOFrozen>()];

		public override SynergyEffects Effects => new(
			[
				
				ClearBuff.Create<AOBurning>(),
				
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<Petrified>(), // petrified
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<AOBleed>(),
			],
			[
				Synergy.Create<CloudyEffect>(1.1f),
				Synergy.Create<Crystallized>(0.9f),
				
				Synergy.Create<AOBurning>(.9f),
				Synergy.Create<CharredEffect>(1.125f),
				Synergy.Create<FreezingEffect>(1.1f),
				
				Synergy.Create<AOPoisoned>(.9f),
				Synergy.Create<SandyEffect>(0.9f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<Scalding>(0.9f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

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
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 0f, 0f, 0, Color.DimGray, 1.5f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.75f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), 0, Color.DimGray, 4f * intensity)];
				spawnedDust.noGravity = true;
				Dust.NewDust(position, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.DimGray, 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
