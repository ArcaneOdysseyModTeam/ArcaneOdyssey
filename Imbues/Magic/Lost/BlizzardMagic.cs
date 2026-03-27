using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class BlizzardMagic : AOMagic
	{
		public override float Aura => .9f;
		public override float? DashResist => 1.075f;
		public override bool? Cold => true;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => Color.DarkGray;
		public override Color ImbueColour2 => Color.White;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ImbueSpeed => .925f;
		public override float ImbueSize => 1.15f;
		public override float ImbueDamage => 1f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<SnowyEffect>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, AOFrozen>(), Combo.Create<FreezingEffect, AOFrozen>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBurning>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<SearedEffect>()
			],
			[
				Synergy.Create<Crystallized>(0.8f),
				Synergy.Create<AOBleed>(1.05f),
				Synergy.Create<AOBurning>(.9f),
				Synergy.Create<CharredEffect>(0.8f),
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<FreezingEffect>(1.1f),
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Scorched>(0.8f),
				Synergy.Create<Soaked>(1.1f),
				Synergy.Create<SearedEffect>(0.8f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Snow, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Snow, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowBlock, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowBlock, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
