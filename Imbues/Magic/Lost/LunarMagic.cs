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
	public class LunarMagic : MagicType
	{
		public override float Aura => .8f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => true;
		public override Color ImbueColour => new(0, 10, 87);
		public override Color ImbueColour2 => new(137, 64, 255);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override float ImbueSpeed => 1.1f;
		public override float ImbueSize => 1.25f;
		public override float ImbueDamage => 0.95f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Soaked>(60 * 7), Debuff.Create<BlindedEffect>(3 * 60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
				ClearBuff.Create<Burning>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<SearedEffect>()
			],
			[
				Synergy.Create<Bleeding>(1.05f),
				Synergy.Create<Burning>(.8f),
				Synergy.Create<CharredEffect>(0.9f),
				Synergy.Create<DrainedEffect>(0.9f),
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<FreezingEffect>(1.075f),
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<SandyEffect>(0.8f),
				Synergy.Create<Scorched>(0.7f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<SearedEffect>(0.7f)
			]
		);

		public override int BlastFrames => 5;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_GlowingMushroom, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.MushroomTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_GlowingMushroom, Scale: 1.2f * area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, Scale: area.RelativeScale());
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.MushroomTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water_GlowingMushroom, (Main.rand.NextFloat() - 0.5f) * (25f * ScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * ScrollSize), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(position, 0, 0, DustID.RedsWingsRun, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), 150, Scale: 3f * intensity)];
				spawnedDust1.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_GlowingMushroom, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<WaterMagic>();
		}
	}
}
