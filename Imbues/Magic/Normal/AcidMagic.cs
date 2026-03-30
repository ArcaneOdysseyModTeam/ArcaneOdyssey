using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class AcidMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterMutation<OilMagic>();
		}
		public override bool Special => true;
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => Color.Purple;
		public override float ImbueSpeed => 0.925f;
		public override float ImbueSize => 1f;
		public override float ImbueDamage => 1f;
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.05f;
		public override float ScrollDamage => 0.875f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Corroding>(60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<SandyEffect>()
			],
			[
				Synergy.Create<AOBleed>(1.075f),
				
				Synergy.Create<AOBurning>(1.075f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<FreezingEffect>(1.2f),
				
				Synergy.Create<Melting>(1.05f),
				
				Synergy.Create<AOPoisoned>(1.05f),
				Synergy.Create<Scorched>(1.1f),
				Synergy.Create<Singed>(1.1f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(1.05f),
				Synergy.Create<Crystallized>(0.9f),
				Synergy.Create<SandyEffect>(0.99f),
				Synergy.Create<Scalding>(1.075f),
				Synergy.Create<SearedEffect>(1.1f)
			]
			);

		public override float Aura => .8f;

		public override int BlastFrames => 5;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UnholyWater, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Venom, Scale: area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UnholyWater, Scale: 1.6f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Venom, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: intensity);
				Dust.NewDust(position, 0, 0, DustID.UnholyWater, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UnholyWater, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}