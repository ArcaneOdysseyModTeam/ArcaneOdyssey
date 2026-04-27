using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.Helpers;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class AncientLightningMagic : MagicType
	{
		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };
		public override Color ImbueColour => Color.Red;
		public override Color ImbueColour2 => Color.DarkRed;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ScrollSpeed => 1.4f;
		public override float ScrollSize => 1.1f;
		public override float ScrollDamage => .875f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Paralyzed>(60, 16), Debuff.Create<AncientLightingChain>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>()];

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Petrified>(), // petrified
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create<AOFrozen>()
			],
			[
				Synergy.Create<FreezingEffect>(1.2f), // frozen
				Synergy.Create<AOBleed>(1.2f), // bleeding
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Soaked>( 1.05f), // 
				Synergy.Create<Flammable>(0.96f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override int BlastFrames => 6;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CrimsonTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{// WAHT IS  THIS IM SO CONFUSED
			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.TheDestroyer, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 0, 0);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CrimsonTorch, Scale: .4f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Firework_Red, (Main.rand.NextFloat() - 0.5f) * (13f * intensity), (Main.rand.NextFloat() - 0.5f) * (13f * intensity), Scale: 2.3f * intensity);
				dust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CrimsonTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.5f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<LightningMagic>();
		}
	}
}
