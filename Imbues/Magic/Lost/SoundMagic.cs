using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class SoundMagic : MagicType
	{
		public override float Aura => .4f;
		public override Color ImbueColour => new(94, 236, 255);
		public override Color ImbueColour2 => Color.White;
		public override bool AnimatedColours => true;
		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.Roar;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;

		public override float ScrollSpeed => 1.4f;
		public override float ScrollSize => 1.25f;
		public override float ScrollDamage => .9f;
		public override float KBMulti => 1.5f;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Monolith;

		public override SynergyEffects Effects => new(
			[

			],
			[
				Synergy.Create<SandyEffect>(.9f),
				Synergy.Create<Crystallized>(1.1f),
				Synergy.Create<Frozen>(1.2f),
			]);

		public override int BlastFrames => 3;

		public const int DustCount = 30;
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < (DustCount * 2); i++)
				{
					var centre = (MathHelper.TwoPi / (DustCount * 2) * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * area.RelativeScale() * 2.5f);
					var dust = Dust.NewDustPerfect(area.Center(), DustID.MushroomTorch, centre / (DustCount * 2 * .75f), Scale: area.RelativeScale());
					dust.noGravity = true;
				}
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (float i = 0; i < DustCount; i++)
			{
				float e = Main.rand.Next(13, 18);
				var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (300f * intensity);
				var dust = Dust.NewDustPerfect(position, DustID.MushroomTorch, (centre / e) * .4f, Scale: 2f * intensity);
				dust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < DustCount; i++)
				{
					var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * area.RelativeScale());
					var dust = Dust.NewDustPerfect(area.Center(), DustID.MushroomTorch, centre / (DustCount * .75f), Scale: area.RelativeScale());
					dust.noGravity = true;
				}
			}
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(ImbueSound, area.Center());
			}
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<WindMagic>();
		}
	}
}
