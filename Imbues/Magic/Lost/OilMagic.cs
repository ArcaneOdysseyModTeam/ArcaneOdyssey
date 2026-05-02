using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class OilMagic : MagicType
	{
		public override float Aura => .8f;
		public override float DashSpeed => 1.2f; // burst
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(20, 20, 20); // lerp between purple and gray quickly, more commonly gray
		public override Color ImbueColour2 => Color.Black;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override float ScrollSpeed => 1.1f;
		public override float ScrollSize => 1.25f;
		public override float ScrollDamage => 1.28f;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Segmented;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Flammable>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<Burning>(1.15f),
				Synergy.Create<Melting>(1.15f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Bleeding>(1.1f),
				Synergy.Create<HeavyBleed>(1.1f),
				Synergy.Create<SandyEffect>(0.96f),
				Synergy.Create<SnowyEffect>(0.96f),
				Synergy.Create<CharredEffect>(1.05f),
				Synergy.Create<SearedEffect>(1.1f)
			]
			);

		public override int BlastFrames => 5;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Cavern, direction.X * 2f, direction.Y * 2f, 0, Color.Black, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Cavern, 0f, 0f, 0, Color.Black, 1.2f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water_Cavern, (Main.rand.NextFloat() - 0.5f) * (25f * intensity), (Main.rand.NextFloat() - 0.5f) * (45f * intensity), 0, Color.Black, 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Cavern, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.Black, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<EarthMagic>();
		}
	}
}