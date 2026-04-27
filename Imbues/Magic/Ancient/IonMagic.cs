using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Ancient
{
	public class IonMagic : MagicType
	{
		public override bool ImmuneDash => true; // instant
		public override bool? Cold => false;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Ancient;
		public override SoundStyle? ImbueSound => SoundID.Item91;
		public override Color ImbueColour => new(0, 255, 0);
		public override bool CanBeWet => false;
		public override float ScrollSpeed => 1.5f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 1.6f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<IonizedEffect>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Soaked>()
			],
			[
				Synergy.Create<AOBleed>(1.15f),
				Synergy.Create<AOBurning>(1.075f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.99f),
				Synergy.Create<FreezingEffect>(0.97f),
				Synergy.Create<Melting>(1.05f),
				Synergy.Create<AOPoisoned>(1.05f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Soaked>(0.95f),
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<Scalding>(1.075f),
				Synergy.Create<SearedEffect>(1.1f),
				Synergy.Create<Scorched>(1.1f)
			]
			);

		public override int BlastFrames => 4;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CursedTorch, direction.X * 0.4f, direction.Y * 0.4f, Scale: 3f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CursedTorch, Scale: 3f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.GreenFairy, (Main.rand.NextFloat() - 0.5f) * (6f * intensity), (Main.rand.NextFloat() - 0.5f) * (5f * intensity), Scale: 3f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CursedTorch, 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<EnergyMagic>();
		}
	}
}