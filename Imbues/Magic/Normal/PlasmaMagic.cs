using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class PlasmaMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterMutation<AetherMagic>();
			RegisterMutation<EnergyMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<PhoenixMagic>();
			RegisterMutation<SunMagic>();
			RegisterMutation<AetherLightningMagic>();
			RegisterMutation<RegulusMagic>();
		}
		public override bool Special => true;
		public override bool ImmuneDash => true; // instant
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Item91;
		public override Color ImbueColour => new Color(255, 100, 255, 255);
		public override bool CanBeWet => false;
		public override float ImbueSpeed => 1.125f;
		public override float ImbueSize => 0.948f;
		public override float ImbueDamage => 0.9f;
		public override float ScrollSpeed => 1.2f;
		public override float ScrollSize => 1f;
		public override float ScrollDamage => 0.825f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Scorched>(60 * 10)];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Bleeding>(1.15f),
				
				Synergy.Create<Burning>(1.075f),
				Synergy.Create<CharredEffect>(1.1f),
				
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.99f),
				Synergy.Create<FreezingEffect>(0.97f),
				
				Synergy.Create<Melting>(1.05f),
				
				Synergy.Create<Poisoned>(1.05f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Singed>(1.1f),
				Synergy.Create<Soaked>(0.95f),
				
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<Scalding>(1.075f),
				Synergy.Create<SearedEffect>(1.1f)
			]
			);

		public override int BlastFrames => 4;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Solar;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.PinkTorch, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.PinkTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.ShadowbeamStaff, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.Firework_Pink, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity).noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.ShadowbeamStaff, 5f * (Main.rand.NextFloat() - 0.5f), 5f * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}