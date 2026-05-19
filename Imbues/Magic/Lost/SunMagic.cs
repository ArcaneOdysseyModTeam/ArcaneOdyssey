using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.Helpers;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class SunMagic : MagicType
	{
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = false; }
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(255, 124, 43);
		public override Color ImbueColour2 => new(227, 73, 52);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;
		public override bool CanBeWet => false;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Solar;
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.3f;
		public override float ScrollDamage => 1.2f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<ProminenceDebuff>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Bleeding>(1.15f),
				Synergy.Create<CharredEffect>(1.01f),
				
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.85f),
				Synergy.Create<FreezingEffect>(0.99f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Soaked>(0.99f),
				
				Synergy.Create<Melting>(1.05f),
				
				Synergy.Create<Scorched>(1.1f),
				
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<SandyEffect>(0.98f),
				Synergy.Create<Scalding>(1.1f),
				Synergy.Create<SearedEffect>(1.1f)

			]
			);

		public override int BlastFrames => 4;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Torch, direction.X * 2f, direction.Y * 2f, Scale: 5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Torch, Scale: 2f * area.RelativeScale());
			}

		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Flare, (Main.rand.NextFloat() - 0.5f) * (35f * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * intensity), Scale: 8f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Torch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 8f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<MagmaMagic>();
		}
	}
}
